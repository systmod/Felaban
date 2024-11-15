using Common.Domain.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repositories;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using ConcentratorFraud.Felaban.Auth.Domain.Services;


namespace ConcentratorFraud.Felaban.Auth.BusinessLogic.Services
{
    public class RolesService: IRolesService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<RolesService> _logger;
        private readonly IEntityRepository<DetallePerfil> _detallePerfil;
        private readonly IEntityRepository<Perfil> _perfil;

        public RolesService(
                IMapper mapper,
                IConfiguration appSettings,                
                AuthContext db,
                IAuthContextProcedures proc,
                ILogger<RolesService> logger,
                IEntityRepository<DetallePerfil> detallePerfil,
                IEntityRepository<Perfil> perfil
            )
        {
            _mapper = mapper;
            _appSettings = appSettings;            
            _procedures = proc;            
            _db = db;
            _logger = logger;
            _detallePerfil = detallePerfil;
            _perfil = perfil;
        }

        public async Task<IOperationResult<List<RolesDto>>> GetRoles(IOperationRequest model)
        {
            var roles = _db.Perfil.Include(x => x.IdTipoPerfilNavigation)
                                  .Where(x => x.IdProducto == model.Aplicacion.Producto.IdProducto)
                                  .Select(x => _mapper.Map<RolesDto>(x))
                                  .ToList();

            return await roles.ToResultAsync();
        }

        public async Task<IOperationResult<List<RolesDto>>> GetRolesByAplicacion(IOperationRequest model, int idAplicacion)
        {
            var roles = _db.Perfil.Include(x => x.IdTipoPerfilNavigation)
                                  .Where(x => x.IdAplicacion == idAplicacion)
                                  .Select(x => _mapper.Map<RolesDto>(x))
                                  .ToList();

            return await roles.ToResultAsync();
        }

        public async Task<IOperationResult<List<ProfileDetailDto>>> GetDetailsByIdPerfil(string idPerfil)
        {
            try
            {
                Guid.TryParse(idPerfil, out var id);
                var detalles = _db.DetallePerfil.Search(x => x.IdEstado && x.IdPerfil == id).Include(X => X.IdOpcionNavigation);

                return await _mapper.Map<List<ProfileDetailDto>>(detalles)
                                    .ToResultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: Obtener Usuario por Id");
                return new OperationResult<List<ProfileDetailDto>>(HttpStatusCode.BadRequest, "No se encontro el usuario!", error: $"{ex}");
            }
        }

        public async Task<IOperationResultList<ProfileDetailDto>> AssignOptions(string idPerfil, IOperationRequest<List<ProfileDetailRequest>> request)
        {
            try
            {
                var perfil = _db.Perfil.FirstOrDefault(x => x.IdPerfil.ToString() == idPerfil && x.IdEstado == 1);

                if (perfil == null)
                {
                    return new OperationResultList<ProfileDetailDto>(HttpStatusCode.NotFound, "El perfil no existe!");
                }

                foreach (var item in request.Data)
                {
                    var detalle = _mapper.Map<DetallePerfil>(item);

                    detalle.IdPerfil = Guid.Parse(idPerfil);
                    perfil.DetallePerfil.Add(detalle);
                }

                await _detallePerfil.InsertAllAsync(perfil.DetallePerfil);
                await _detallePerfil.SaveAsync(request);

                var result = _mapper.Map<List<ProfileDetailDto>>(perfil.DetallePerfil);

                return await result.ToResultListAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultListAsync<ProfileDetailDto>();
            }
        }

        public async Task<IOperationResult> DeleteDetail(long id, IOperationRequest request)
        {
            var detail = _db.DetallePerfil.FirstOrDefault(x => x.IdEstado && x.IdDetallePerfil == id);

            if (detail != null)
            {
                await _detallePerfil.DeleteAsync(detail);
                await _db.SaveAsync(request);

                return new OperationResult(HttpStatusCode.OK);
            }

            return new OperationResult(HttpStatusCode.NotFound, "No se encontró el detalle especificado!");
        }

        public async Task<IOperationResult<PerfilDto>> PostPerfil(IOperationRequest<PerfilRequest> request)
        {
            try
            {               
                var rol = _mapper.Map<Perfil>(request.Data);
                await Task.WhenAll(
                    _perfil.InsertAsync(rol),
                    _perfil.SaveAsync(request)
                    );
               
                var result = _mapper.Map<PerfilDto>(rol);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<PerfilDto>();
            }
        }

        public async Task<IOperationResult<PerfilDto>> PutPerfil(IOperationRequest<PerfilRequest> request, Guid id)
        {
            try
            {
                var rol = _perfil.Search(x => x.IdPerfil == id).FirstOrDefault();
                rol = _mapper.Map(request.Data, rol);

                await Task.WhenAll(
                    _perfil.InsertAsync(rol),
                    _perfil.SaveAsync(request)
                    );

                var result = _mapper.Map<PerfilDto>(rol);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<PerfilDto>();
            }
        }

        public async Task<IOperationResult> DeletePerfil(IOperationRequest request, Guid id)
        {
            var perfil = _perfil.FirstOrDefault(x => x.IdEstado==1 && x.IdPerfil == id);

            if (perfil != null)
            {
                await _perfil.DeleteAsync(perfil);
                await _perfil.SaveAsync(request);

                return new OperationResult(HttpStatusCode.OK);
            }

            return new OperationResult(HttpStatusCode.NotFound, "No se encontró el Perfil!");
        }

    }
}
