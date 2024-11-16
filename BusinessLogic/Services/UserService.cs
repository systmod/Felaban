using AutoMapper;
using Azure.Core;
using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Storage.Handlers;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Configurations;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NPOI.HSSF.Record.Aggregates;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IEntityRepository<DetallePerfil> _detallePerfil;
        private readonly FileHandler _fileHandler;
        private readonly ITools _tools;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<UsuarioUnidad> _usuarioUnidad;
        private readonly IEntityRepository<UserMailVerify> _userMailVerify;
        private readonly IEntityRepository<Usuario> _user;

        private string SecretKey => _appSettings["SecretKey"];

        public UserService(
                IMapper mapper,
                IConfiguration appSettings,
                ILogger<UserService> logger,
                AuthContext db,
                IAuthContextProcedures proc,
                IEntityRepository<DetallePerfil> detallePerfil,
                FileHandler fileHandler,
                ITools tools,
                IEntityRepository<UsuarioUnidad> usuarioUnidad,
                IEntityRepository<UserMailVerify> userMailVerify,
                IEntityRepository<Usuario> user)
        {
            _mapper = mapper;
            _appSettings = appSettings;
            _logger = logger;
            _procedures = proc;
            _detallePerfil = detallePerfil;
            _fileHandler = fileHandler;
            _db = db;
            _tools = tools;
            _usuarioUnidad = usuarioUnidad;
            _userMailVerify = userMailVerify;
            _user = user;
        }

        public async Task<IOperationResult<AuthenticationResponse>> Authenticate(LoginRequest request)
        {
            if (!Guid.TryParse(request.AppId, out var applicationToken))
            {
                return new OperationResult<AuthenticationResponse>(HttpStatusCode.Unauthorized, "La aplicacion especificada no esta autorizada para utilizar este servicio");
            }

            var aplicacion = _db.Aplicacion.Include(x => x.IdProductoNavigation).FirstOrDefault(x => x.Token == applicationToken);
            var res = await _procedures.IniciarSesionAsync(request.Usuario, request.Clave, request.AppId, request.DeviceId, request.Latitud, request.Longitud);

            var data = res.FirstOrDefault();

            if (data != null)
            {
                var result = data as IOperationResult;

                var dto = (_mapper.Map<UsuarioDto>(data));

                AuthenticationResponse response = default;

                if (!string.IsNullOrEmpty(dto.IdUsuario))
                {
                    var changePassword = result.StatusCode == HttpStatusCode.PreconditionRequired;

                    if (aplicacion.Token.ToString() == dto.Token)
                    {
                        aplicacion.Directo = true;

                        dto.Status = TokenPermissionEnum.Authorized;
                        
                        var empresa = _db.Empresa.FirstOrDefault(x => request.Clave == x.Token.ToString());

                        dto.Empresa = _mapper.Map<EmpresaDto>(empresa);
                    }
                    else
                    {
                        // Validamos si el usuario tiene acceso a esta aplicacion:                                                                             
                        if (!_db.UsuarioUnidad.Any(x => x.IdUsuario == dto.IdUsuario && x.IdAplicacion == aplicacion.IdAplicacion))
                        {
                            return new OperationResult<AuthenticationResponse>(HttpStatusCode.Forbidden, "El usuario no esta autorizado para utilizar para esta aplicación.");
                        }
                        //Obtengo los datos de la empresa
                        var uunidad = _usuarioUnidad.Search(x => x.IdUsuario == dto.IdUsuario && x.IdAplicacion == aplicacion.IdAplicacion).FirstOrDefault();
                        var empresa = _db.Empresa.FirstOrDefault(x => x.IdEmpresa == uunidad.IdEmpresa);
                        dto.Empresa = _mapper.Map<EmpresaDto>(empresa);

                        dto.Status = TokenPermissionEnum.Login;
                    }

                    dto.Aplicacion = _mapper.Map<AplicacionDto>(aplicacion);

                    // TODO: Terminar de obtener los roles por usuario
                    var roles = _db.UsuarioUnidad.Where(x => x.IdUsuario == dto.IdUsuario && x.IdAplicacion == dto.Aplicacion.IdAplicacion);

                    if (roles != null)
                    {
                        var perfilRol = _db.Perfil.Where(x => x.IdPerfil == roles.FirstOrDefault().IdPerfil).ToList();
                        dto.Roles = new List<string>();
                        foreach (var role in perfilRol)
                        {
                            dto.Roles.Add(role.Descripcion);
                        }
                        dto.IdPerfil = perfilRol.FirstOrDefault().IdPerfil;
                    }

                    var estado = aplicacion.Directo ? TokenPermissionEnum.Authorized : TokenPermissionEnum.Login;
                    var usertoken = dto.GenerateJwtToken(SecretKey, estado);

                    response = new AuthenticationResponse(dto, usertoken);
                }

                return await response.ToResultAsync(result.StatusCode, result.Message, result.Error);
            }

            return new OperationResult<AuthenticationResponse>(HttpStatusCode.Unauthorized, "No Autorizado");
        }


        public async Task<IOperationResult<AuthorizationResponse>> Authorize(IOperationRequest<AuthorizationRequest> request)
        {
            try
            {

                Guid.TryParse(request.Data.IdUnidadAdmin, out var uid);

                var unidadAdmin = _db.UnidadAdmin
                                        .Include(x => x.IdTipoUnidadAdminNavigation)
                                        .Include(x => x.IdEmpresaNavigation)
                                        .FirstOrDefault(x => x.Token == uid);

                if (unidadAdmin == null)
                {
                    return new OperationResult<AuthorizationResponse>(HttpStatusCode.BadRequest, "Los datos ingresados son inválidos!");
                }

                var empresa = _mapper.Map<EmpresaDto>(unidadAdmin.IdEmpresaNavigation);
                var local = _mapper.Map<UnidadAdminDto>(unidadAdmin);
                var aplicacion = request.Aplicacion as AplicacionDto;

                var usuario = new UsuarioDto(request.Usuario, aplicacion, empresa, local);

                var usertoken = usuario.GenerateJwtToken(SecretKey, TokenPermissionEnum.Authorized);

                var response = new AuthorizationResponse(usuario, usertoken);

                return await response.ToResultAsync(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new OperationResult<AuthorizationResponse>(HttpStatusCode.Unauthorized, "No Autorizado", default, ex.Message);
            }
        }



        public async Task<IOperationResult<UsuarioDto>> ResetPassword(string username, int? idProducto)
        {
            try
            {
                var user = await GetUserAsync(username, idProducto);
                var userDto = (_mapper.Map<UsuarioDto>(user));

                var token = _db.Usuario.FirstOrDefault(x => x.IdUsuario == user.IdUsuario)?.Token;

                userDto.Token = $"{token}:{DateTime.Now.AddDays(1).ToBinary()}".ToBase64String();
                var newPassword = $"{token.ToString().Split('-').Last()}C*";
                newPassword = ChangePassword(token.ToString().ToUpper(), newPassword, true).Result.Result.Token;
                userDto.Hash = newPassword;

                return await userDto.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<UsuarioDto>(ex);
            }
        }

        public async Task<IOperationResult<List<UnidadAdminDto>>> GetAdminUnitsByUserAndCompany(IOperationRequest<int> model)
        {
           
            var res = await _procedures.Custom_AdminUnitsByCompanyAsync(model.Usuario.IdUsuario, model.Data);
            var data = res.AsQueryable();

            var result = _mapper.Map<List<UnidadAdminDto>>(data);


            return await result.ToResultAsync();
        }

        public async Task<IOperationResult<List<UnidadAdminDto>>> GetAdminUnitsByUser(IOperationRequest<string> model)
        {
            var unitsByUser = _db.UsuarioUnidad.Where(x => x.IdUsuario == model.Usuario.IdUsuario
                                            && x.IdAplicacion == model.Aplicacion.IdAplicacion)
                                .Select(x => x.IdUnidadAdmin);

            var units = _db.UnidadAdmin.Include(x => x.IdEmpresaNavigation)
                                        .Include(x => x.IdTipoUnidadAdminNavigation)
                                        .Where(x => unitsByUser.Contains(x.IdUnidadAdmin));

            if (!string.IsNullOrEmpty(model.Data))
            {
                var tipoUnidad = _db.TipoUnidadAdmin.Where(x => x.Codigo == model.Data).Select(x => x.IdTipoUnidadAdmin);

                units = units.Where(x => model.Data == null || tipoUnidad.Contains(x.IdTipoUnidadAdmin));
            }

            return await units.Select(x => _mapper.Map<UnidadAdminDto>(x))
                                        .ToList().ToResultAsync();
        }

        private async Task<Usuario> GetUserAsync(string username, int? idProducto)
        {
            Usuario user = null;
            
            if(idProducto != 0)
            {
                user = _db.Usuario.FirstOrDefault(x => (x.Correo == username || x.Token.ToString() == username) && x.IdProducto == idProducto);
            }
            else
            {
                user = _db.Usuario.FirstOrDefault(x => x.InicioSesion == username || x.Token.ToString() == username);
            }

            if (user == null)
            {
                throw new Exception("El usuario especificado no existe!");
            }

            return await Task.FromResult(user);
        }


        public async Task<IOperationResult<UsuarioDto>> ChangePassword(string token, string password, bool habilitado = true)
        {
            if (habilitado && !PasswordIsValid(password))
            {
                return new OperationResult<UsuarioDto>(System.Net.HttpStatusCode.BadRequest, "La clave especificada no es válida!");
            }

            var data = await _procedures.ActualizarPasswordAsync(token, password, true);

            var user = data.FirstOrDefault();

            if (user == null)
            {
                return new OperationResult<UsuarioDto>(System.Net.HttpStatusCode.BadRequest, "Hubo un error al cambiar la clave del usuario");
            }

            var result = (user as IOperationResult) ?? new OperationResult(System.Net.HttpStatusCode.BadRequest, "Hubo un error al cambiar la clave del usuario");

            var userDto = (_mapper.Map<UsuarioDto>(user));
            userDto.Token = password;

            return await userDto.ToResultAsync(result.StatusCode, result.Message, result.Error);
        }

        public async Task<IOperationResult<UsuarioDto>> GetByLogin(string login)
        {
            var user = _db.Usuario.FirstOrDefault(x => x.InicioSesion == login);

            if (user != null)
            {
                var result = _mapper.Map<UsuarioDto>(user);

                return await (result).ToResultAsync();
            }

            return new OperationResult<UsuarioDto>(HttpStatusCode.BadRequest, "No se encuentra el usuario especificado");
        }

        public async Task<IOperationResult<UsuarioDto>> GetByToken(string token)
        {
            var user = _db.Usuario.FirstOrDefault(x => x.IdEstado && x.Token.ToString() == token);

            UsuarioDto result;

            if (user == null)
            {
                // Validamos si el usuario es un servicio:
                var app = _db.Aplicacion.FirstOrDefault(x => x.Token.ToString() == token);
                if (app == null)
                {         
                    return new OperationResult<UsuarioDto>(HttpStatusCode.BadRequest, "No se encuentra el usuario especificado");
                }

                // Configuramos el usuario de servicios:
                result = new UsuarioDto
                {
                    Status = TokenPermissionEnum.Authorized,
                    Token = token,
                    Nombre = app.Nombre
                };
            }
            else
            {
                result = _mapper.Map<UsuarioDto>(user);
            }
                  
            return await result.ToResultAsync();
        }

        public async Task<IOperationResult<UnidadAdminDto>> GetUnidadAdminByToken(string token)
        {
            Guid.TryParse(token, out Guid tokenUID);

            var result = _db.UnidadAdmin.Include(x => x.IdTipoUnidadAdminNavigation).FirstOrDefault(x => x.Token == tokenUID);
            return await _mapper.Map<UnidadAdminDto>(result).ToResultAsync();
        }

        public async Task<IOperationResult<AplicacionDto>> GetAplicacionByToken(string token)
        {
            var result = _db.Aplicacion.Include(x => x.IdProductoNavigation).FirstOrDefault(x => x.Token.ToString() == token);

            return await _mapper.Map<AplicacionDto>(result).ToResultAsync();
        }

        public async Task<IOperationResult<UsuarioDto>> Register(IOperationRequest<UserRegisterRequest> request)
        {
            var clave = "";
            if (request.Data.Clave.IsNullOrEmpty())            
                clave = this.GeneratePassword();            
            else            
                clave = request.Data.Clave;

            int idEmpresa = 1;
            if (request.Data.IdEmpresa > 0)
                idEmpresa = (int)request.Data.IdEmpresa;
            else
                idEmpresa = request.Empresa.IdEmpresa;

            var res = await _procedures.AgregarUsuarioAsync(request.Data.Identificacion, request.Data.InicioSesion, request.Data.Nombres,
                                        request.Data.Correo, request.Data.Celular, clave, 
                                        request.Aplicacion.IdAplicacion, request.Data.IdPerfil,
                                        idEmpresa , request.Data.IdLocal);

            

            var data = res.FirstOrDefault();

            if (data != null)
            {

                if (!string.IsNullOrEmpty(request.Data.Foto) && request.Data.Foto != "string")
                {
                    var user = _db.Usuario.FirstOrDefault(x => x.IdEstado && x.IdUsuario == data.IdUsuario);
                    var path = $"Usuario/{user?.IdUsuario}.jpg";
                    var url = await UploadFile(request.Data.GetBytes(), path);
                    user.Foto = url.Result;
                    await _db.Usuario.UpdateAsync(user);
                }

                var result = data as IOperationResult;

                if (!result.Success)
                {
                    return new OperationResult<UsuarioDto>(result.StatusCode, result.Message, default, result.Error);
                }

                var usuario = _mapper.Map<UsuarioDto>(data);
                usuario.Hash = clave;
                usuario.Aplicacion = new AplicacionDto();
                usuario.Aplicacion.IdAplicacion = request.Aplicacion.IdAplicacion;

                return await usuario.ToResultAsync(result.StatusCode, result.Message, result.Error);
            }

            return new OperationResult<UsuarioDto>(HttpStatusCode.Unauthorized, "No Autorizado");
        }

        public async Task<IOperationResult<string>> UploadFile(byte[] array, string path)
        {
            try
            {
                var file = array;
                var stream = new MemoryStream(file);
                await _fileHandler.UploadAsync(path, stream);
                var url = await _fileHandler.GetFile(path);
                return await url.Result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<string>(ex);
            }
        }

        private static bool PasswordIsValid(string password)
        {
            return (!string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{8,64})"));
        }

        public async Task<IOperationResult<UsuarioDto>> GetUserById(string id)
        {
            try
            {
                var result = _db.Usuario.FirstOrDefault(x => x.IdEstado && x.IdUsuario == id);

                return await _mapper.Map<UsuarioDto>(result).ToResultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: Obtener Usuario por Id");
                return new OperationResult<UsuarioDto>(HttpStatusCode.BadRequest, "No se encontro el usuario!");
            }

        }

        public async Task<IOperationResult<UsuarioDto>> GetUserProfile(string userLogin, IOperationRequest model)
        {
            try
            {
                var user = _db.Usuario.FirstOrDefault(x => x.IdEstado && x.InicioSesion == userLogin);
                

                var result = _mapper.Map<UsuarioDto>(user);

                var detalles = GetDetallePerfil(model, user?.IdUsuario);

                // Cargamos los permisos solo si tiene acceso.
                if (detalles.Any())
                {
                    var perfil = detalles.Select(x => x.IdPerfilNavigation).FirstOrDefault();
                    var tipoPerfil = _db.TipoPerfil.FirstOrDefault(x => x.IdTipoPerfil == perfil.IdTipoPerfil);
                    var options = detalles.Select(x => x.IdOpcionNavigation).Distinct();
                    var modules = _db.Modulo.Search(x => x.IdEstado == 1)
                                            .Select(x => _mapper.Map<ModuloDto>(x))
                                            .ToList();

                    foreach (var item in modules)
                    {
                        item.Opciones = options.Where(x => x.IdModulo == item.IdModulo)
                                                .Select(x => (IOptionEntity)_mapper.Map<OpcionDto>(x))
                                                .ToList();
                    }

                    modules = modules.Where(x => x.Opciones != null && x.Opciones.Any()).ToList();

                    result.Perfil = _mapper.Map<PerfilDto>(perfil);
                    result.Perfil.TipoPerfil = _mapper.Map<TipoPerfilDto>(tipoPerfil);
                    result.Perfil.Permisos = modules;

                }
                  
                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<UsuarioDto>(HttpStatusCode.BadRequest, "No se encontro el usuario!", error: $"{ex}");
            }
        }

        private List<DetallePerfil> GetDetallePerfil(IOperationRequest model, string iduser)
        {
            var appid = model.Aplicacion?.IdAplicacion ?? -1;
            var permisos = _db.UsuarioUnidad.Search(x => x.IdUsuario == iduser && x.IdAplicacion == appid);

            if (model.Local?.IdUnidadAdmin > 0)
            {
                permisos = permisos.Where(x => x.IdUnidadAdmin == model.Local.IdUnidadAdmin);
            }
            else   
            {
                permisos = permisos.Where(x => x.IdEmpresa == model.Empresa.IdEmpresa);
            }

            if (!permisos.Any())
            {
                throw new Exception("El usuario no tiene permisos!");
            }

            var perfil = permisos.FirstOrDefault();

            var detalles = _db.DetallePerfil.Search(x => x.IdPerfil == perfil.IdPerfil)
                .Include(x => x.IdPerfilNavigation)
                .Include(x => x.IdOpcionNavigation)
                .ToList();

            return detalles;
        }

        public async Task<IOperationResultList<UnidadAdminDto>> GetAdminUnitsByCompany(IOperationRequest model, string term, int page = 1, int pageSize = 10)
        {
            var units = _db.UnidadAdmin.Include(x => x.IdEmpresaNavigation)
                                       .Include(x => x.IdTipoUnidadAdminNavigation)
                                       .Where(x => x.IdEmpresa == model.Empresa.IdEmpresa);

            if (!string.IsNullOrEmpty(term))
            {
                units = units.Where(x => x.Descripcion.Contains(term));
            }

            return await units.ToResultListAsync<UnidadAdmin, UnidadAdminDto>(page, pageSize);
        }

        public async Task<IOperationResult<UsuarioUnidadDto>> AssignAdminUnit(IOperationRequest<AssignUnitRequest> model)
        {
            try
            {
                var perfil = _db.Perfil.FirstOrDefault(x => x.IdPerfil == model.Data.IdPerfil);

                if (perfil == null)
                {
                    return new OperationResult<UsuarioUnidadDto>(HttpStatusCode.BadRequest, "El perfil especificado no existe!");
                }

                var idUsuario = _db.Usuario.FirstOrDefault(x => x.Token.ToString() == model.Data.IdUsuario || x.IdUsuario == model.Data.IdUsuario)?.IdUsuario;

                if (idUsuario == null)
                {
                    return new OperationResult<UsuarioUnidadDto>(HttpStatusCode.BadRequest, "El usuario especificado no existe!");
                }

                // buscamos la asignación si existe:
                var usuarioUnidad = _db.UsuarioUnidad.FirstOrDefault(x => x.IdUsuario == idUsuario
                                               && x.IdEmpresa == model.Empresa.IdEmpresa
                                               && x.IdAplicacion == model.Aplicacion.IdAplicacion
                                               && x.IdUnidadAdmin == model.Data.IdUnidadAdmin);

                if (usuarioUnidad != null)
                {
                    if (usuarioUnidad.Activo == model.Data.Activo)
                    {
                        return new OperationResult<UsuarioUnidadDto>(HttpStatusCode.BadRequest, "Ya se encuentra asignado a esta sucursal");
                    }

                    usuarioUnidad.Activo = model.Data.Activo;
                    usuarioUnidad.IdPerfil = model.Data.IdPerfil;
                    _db.Update(usuarioUnidad);
                }
                else
                {
                    usuarioUnidad = _mapper.Map<UsuarioUnidad>(model.Data);
                    usuarioUnidad.IdEmpresa = model.Empresa.IdEmpresa;
                    usuarioUnidad.IdAplicacion = model.Aplicacion.IdAplicacion;
                    usuarioUnidad.IdUsuario = idUsuario;
                    _db.Add(usuarioUnidad);
                }

                await _db.SaveAsync(model);

                var unidad = _db.UnidadAdmin.FirstOrDefault(x => x.IdUnidadAdmin == usuarioUnidad.IdUnidadAdmin);                  
                var result = _mapper.Map<UsuarioUnidadDto>(usuarioUnidad);

                return await result.ToResultAsync();

            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<UsuarioUnidadDto>();
            }
        }

        public async Task<IOperationResultList<UsuarioDto>> GetAllUsers(IOperationRequest<FindUsersRequest> model, int page = 1, int? pageSize = 10 )
        {
            try
            {
                var users = await _procedures.ObtenerUsuariosAsync(model.Aplicacion.Token, model.Empresa.Token, model.Data.Term, model.Data.Perfil, model.Data.Unidad);

                var roles = _db.UsuarioUnidad.Include(x => x.IdPerfilNavigation)
                                             .Where(x => users.Select(p => p.IdUsuario)
                                                              .Contains(x.IdUsuario)
                                                        && x.IdEmpresa == model.Empresa.IdEmpresa
                                                        && x.IdAplicacion == model.Aplicacion.IdAplicacion);

                if (model.Data.Perfil != null)
                {
                    roles = roles.Where(x => x.IdPerfil == model.Data.Perfil);
                }

                if (model.Data.Unidad != null)
                {
                    var unidad = _db.UnidadAdmin.First(x => x.Token == model.Data.Unidad)?.IdUnidadAdmin;
                    roles = roles.Where(x => x.IdUnidadAdmin == unidad);
                }


                var result = _mapper.Map<List<UsuarioDto>>(users);
                result.ForEach(x =>
                {
                    x.Roles = roles.Where(x => x.IdUsuario == x.IdUsuario)
                                   .Select(x => x.IdPerfilNavigation.Descripcion)
                                   .Distinct()
                                   .ToList();
                });

                return await result.ToResultListAsync(page, pageSize);
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<UsuarioDto>();
            }
        }


        public async Task<IOperationResult<List<UnidadAdminDto>>> GetAdminUnitsByIdUser(IOperationRequest<string> idUser, string? term = default)
        {
            var idsUnitsByUser = _db.UsuarioUnidad.Where(x => x.IdUsuario == idUser.Data
                                            && x.IdAplicacion == idUser.Aplicacion.IdAplicacion
                                            && x.Activo)
                                .Select(x => x.IdUnidadAdmin);

            var units = _db.UnidadAdmin.Include(x => x.IdEmpresaNavigation)
                                        .Include(x => x.IdTipoUnidadAdminNavigation)
                                        .Where(x => idsUnitsByUser.Contains(x.IdUnidadAdmin));

            if (!string.IsNullOrEmpty(term))
            {
                units = units.Where(x => term == null
                                        || (x.IdTipoUnidadAdminNavigation.Codigo != null && x.IdTipoUnidadAdminNavigation.Codigo.Contains(term))
                                        || (x.Descripcion != null && x.Descripcion.Contains(term)));
            }

            return await units.Select(x => _mapper.Map<UnidadAdminDto>(x))
                                        .ToList().ToResultAsync();
        }

        public async Task<IOperationResult<UsuarioDto>> UpdateUser(string userLogin, IOperationRequest<UserUpdateRequest> request)
        {
            var user = _db.Usuario.FirstOrDefault(x => x.IdEstado && x.InicioSesion == userLogin);

            if(user == null)
            {
                return new OperationResult<UsuarioDto>(HttpStatusCode.NotFound, "Usuario no encontrado");
            }

            user = _mapper.Map<Usuario>(user, request.Data);

            await _db.Usuario.UpdateAsync(user);
            await _db.SaveAsync(request);

            var result = _mapper.Map<UsuarioDto>(user);

            return await result.ToResultAsync();
        }

        public IOperationResult ValidateUsername(string username)
        {
            var exist = _db.Usuario.FirstOrDefault(x => x.InicioSesion == username);

            if (exist == null)
            {
                return new OperationResult(HttpStatusCode.NotFound, "Nombre de usuario no encontrado");
            }

            return new OperationResult(HttpStatusCode.OK, "Ya existe este nombre de usuario");
        }

        #region Empresa

        public async Task<IOperationResult<EmpresaDto>> GetEmpresaByToken(string token)
        {
            var result = _db.Empresa.FirstOrDefault(x => x.Token.ToString() == token);

            return await _mapper.Map<EmpresaDto>(result).ToResultAsync();
        }

        public async Task<IOperationResult<List<EmpresaDto>>> CompaniesByUser(IOperationRequest request)
        {
            var exist = await _db.Empresa
                                      .Where(e => e.IdEstado == 1 && e.UsuarioUnidad.Any(uu => uu.IdUsuario == request.Usuario.IdUsuario))
                                      .Include(e => e.UsuarioUnidad)
                                      .ToListAsync();

            //var exist = _db.UsuarioUnidad.Search(x => x.IdUsuario == request.Usuario.IdUsuario).Include(x => x.IdEmpresaNavigation).ToList();

            if (exist == null)
            {
                return new OperationResult<List<EmpresaDto>>(HttpStatusCode.NotFound, "Usuario no tiene empresas asignadas");
            }

            var result = _mapper.Map<List<EmpresaDto>>(exist);

            return await result.ToResultAsync();
        }


        public IOperationResult CheckMail(string correo, int idAplicacion)
        {
            var exist = _userMailVerify.FirstOrDefault(x => x.Correo == correo && x.IdAplicacion== idAplicacion);

            if (exist == null)
            {
                return new OperationResult(HttpStatusCode.NotFound, "Correo no encontrado", "No existe ese correo señor");
            }

            if (exist.Verificado)
            {
                return new OperationResult(HttpStatusCode.OK, "Correo verificado", "Todo de maravilla :)");
            }
            else
            {
                return new OperationResult(HttpStatusCode.Forbidden, "Correo pendiente de verificación", "El correo esta pendiente cabron");
            }

        }

        public async Task<IOperationResult<string>> GetPlantillaByAplicacion(int idAplicacion)
        {
            var empresa = _db.Aplicacion.FirstOrDefault(x => x.IdAplicacion == idAplicacion);
            return await empresa.PlantillaCorreoVerificacion.ToResultAsync();
        }

        public async Task<IOperationResult<UserMailVerifyDto>> SaveToVerify(IOperationRequest<UserToVerifyRequest> request)
        {
            try
            {
                var mailExists = _userMailVerify.FirstOrDefault(x => x.Correo == request.Data.Correo && x.IdAplicacion == request.Data.IdAplicacion);

                if (mailExists != null && mailExists.Verificado)
                {
                    return new OperationResult<UserMailVerifyDto>(HttpStatusCode.Forbidden, "El correo ya fue verificado anteriormente");
                }

                var newMail = new UserMailVerify
                {
                    Correo = request.Data.Correo,
                    IdAplicacion = request.Data.IdAplicacion,
                    Verificado = false
                };

                if(mailExists == null)
                {
                    await _userMailVerify.InsertAsync(newMail);
                    await _userMailVerify.SaveAsync(request);
                }

                var result = _mapper.Map<UserMailVerifyDto>(newMail);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<UserMailVerifyDto>(ex);
            }
        }

        public async Task<string> VerifyUserMail(long Id)
        {
            var proc = await _procedures.VerifyUserEmailV2Async(Id);
            return proc[0].Message;
        }

        public async Task<IOperationResult> PostLogAudit(IOperationRequest<AuditLogsRequest> request)
        {
            await _procedures.LogAuditAsync(request.Data.InicioSesion, request.Data.IdUsuario, request.Data.IdAplicacion,
                                        request.Data.Browser, request.Data.IpAddress, request.Data.Exception, request.Data.ExecutionDuration,
                                        request.Data.ExecutionDate, request.Data.MethodName,request.Data.Parameters, request.Data.ServiceName,
                                        request.Data.ExceptionMessage);

            return new OperationResult(HttpStatusCode.OK);
        }

        #endregion


        public async Task<IOperationResult<UsuarioDto>> PutUser(IOperationRequest<UsuarioRequest> request, string id)
        {
            try
            {
                var user = _user.Search(x => x.IdUsuario == id).FirstOrDefault();
                user = _mapper.Map(request.Data, user);

                await Task.WhenAll(
                    _user.UpdateAsync(user),
                    _user.SaveAsync(request)
                    );

                var result = _mapper.Map<UsuarioDto>(user);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<UsuarioDto>();
            }
        }

        public async Task<IOperationResult> DeleteUsuario(IOperationRequest request, string id)
        {
            var user = _user.FirstOrDefault(x => x.IdEstado && x.IdUsuario == id);

            if (user != null)
            {
                user.IdEstado = false;
                await _user.UpdateAsync(user);
                await _user.SaveAsync(request);

                return new OperationResult(HttpStatusCode.OK);
            }

            return new OperationResult(HttpStatusCode.NotFound, "No se encontró el Usuario!");
        }

    }
}

