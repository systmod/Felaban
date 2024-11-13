using Common.Domain.Models;
using Common;
using Common.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories;
using Common.Storage.Handlers;
using DataAccess.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace BusinessLogic.Services
{
    public class PermissionsService: IPermissionsService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        public PermissionsService(
                            AuthContext db,
                IAuthContextProcedures proc,
                IMapper mapper
            )
        {
            _db = db;
            _procedures = proc;
            _mapper = mapper;
        }

        public async Task<IOperationResult<List<ModuloDto>>> GetPermissions(IOperationRequest request, string userLogin, string localid)
        {
            try
            {
                var opciones = await _procedures.ObtenerPermisosAsync(request.Aplicacion.Token.ToString(),
                                            userLogin, request.Empresa.Token.ToString(), localid);

                var modulos = _db.Modulo.Where(x => opciones.Select(p => p.IdModulo).Contains(x.IdModulo))
                                            .Select(x => _mapper.Map<ModuloDto>(x))
                                            .ToList();

                foreach (var modulo in modulos)
                {
                    modulo.Opciones = opciones.Where(x => x.IdModulo == modulo.IdModulo)
                                        .Select(x => (IOptionEntity)_mapper.Map<OpcionDto>(x))
                                        .ToList();
                }

                return await modulos.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<List<ModuloDto>>();
            }

        }

        public async Task<IOperationResultList<OpcionDto>> GetOptions(IOperationRequest model, string? term = default, string? idPerfil = default, int page = 1, int pageSize = 10)
        {
            var modules = _db.Modulo.Search(x => x.IdEstado == 1 && x.IdAplicacion == model.Aplicacion.IdAplicacion);

            var options = new List<OpcionDto>();


            foreach (var modulo in modules)
            {
                options = _db.Opcion.Where(x => x.IdModulo == modulo.IdModulo)
                                    .Select(x => _mapper.Map<OpcionDto>(x))
                                    .ToList();
            }

            if (!string.IsNullOrEmpty(idPerfil))
            {
                var detalles = _db.DetallePerfil.Search(x => x.IdEstado && x.IdPerfil.ToString() == idPerfil).ToList();
                var listOpt = options.ConvertAll(item => item);

                foreach (var item in options)
                {
                    var found = detalles.FirstOrDefault(x => x.IdEstado && x.IdOpcion == item.IdOpcion);
                    if (found != null) listOpt.Remove(item);
                }
                options = listOpt;
            }

            if (!string.IsNullOrEmpty(term))
            {
                options = options.Where(x => x.Descripcion.Contains(term)).ToList();
            }

            return await options.ToResultListAsync(page, pageSize);
        }

        public async Task<IOperationResultList<ModuloDto>> GetModulesByIdApplication(IOperationRequest model, string? term = default, int page = 1, int pageSize = 10)
        {
            try
            {
                var opciones = _db.DetallePerfil
                    .Search(x => x.IdPerfil == model.Usuario.IdPerfil)
                    .GroupBy(x => x.IdModulo)
                    .Select(g => g.Key)
                    .ToList();

                var modules = _db.Modulo
                    .Search(x => opciones.Contains(x.IdModulo) && (term == null || x.Descripcion.Contains(term)) && x.IdEstado == 1 && x.IdAplicacion == model.Aplicacion.IdAplicacion)
                    .AsQueryable();
                return await modules.ToResultListAsync<Modulo, ModuloDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return new OperationResultList<ModuloDto>(ex);
            }
        }

        public async Task<IOperationResultList<OpcionDto>> GetOptionsByIdModule(IOperationRequest model,int idModule , string? term = default, int page = 1, int pageSize = 10)
        {
            try
            {
                var opcions = _db.Opcion.Search(x => (term == null || x.Descripcion.Contains(term)) && x.IdEstado == 1 && x.IdModulo == idModule).AsQueryable();
                return await opcions.ToResultListAsync<Opcion, OpcionDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return new OperationResultList<OpcionDto>(ex);
            }
        }

    }
}
