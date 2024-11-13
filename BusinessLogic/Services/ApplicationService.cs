using AutoMapper;
using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ApplicationService: IApplicationService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Aplicacion> _aplicacion;
        public ApplicationService(
                            AuthContext db,
                IAuthContextProcedures proc,
                IMapper mapper,
                IEntityRepository<Aplicacion> aplicacion
            )
        {
            _db = db;
            _procedures = proc;
            _mapper = mapper;
            _aplicacion = aplicacion;
        }

        public async Task<IOperationResult<AplicacionDto>> GuardarAplicacion(IOperationRequest<AplicacionRequest> request)
        {
            try
            {
                var aplicacion = _aplicacion.Search(x => x.IdAplicacion == request.Data.IdAplicacion)
                            .FirstOrDefault();

                if (aplicacion != null)
                {
                    aplicacion = _mapper.Map(request.Data, aplicacion);

                    await Task.WhenAll(
                    _aplicacion.UpdateAsync(aplicacion),
                    _aplicacion.SaveAsync(request));
                }
                else
                {
                    var resultado = _aplicacion.Search(x => x.Nombre == request.Data.Nombre && x.IdProducto == request.Data.IdProducto)
                                .FirstOrDefault();

                    if (resultado != null)
                    {
                        return new OperationResult<AplicacionDto>(HttpStatusCode.NotFound, "Ya existe nombre de aplicación");
                    }

                    aplicacion = _mapper.Map<Aplicacion>(request.Data);

                    await Task.WhenAll(
                    _aplicacion.InsertAsync(aplicacion),
                    _aplicacion.SaveAsync(request));
                }

                var result = _mapper.Map<AplicacionDto>(aplicacion);
                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<AplicacionDto>(ex);
            }
        }

        public async Task<IOperationResult> EliminarAplicacion(IOperationRequest request, int id)
        {
            try
            {
                var aplicacion = await _aplicacion.Search(x => x.IdAplicacion == id && x.IdEstado == 1)
                            .FirstOrDefaultAsync();

                if (aplicacion != null)
                {
                    await _aplicacion.DeleteAsync(aplicacion);
                    await _aplicacion.SaveAsync(request);

                    return new OperationResult(HttpStatusCode.OK);
                }

                return new OperationResult(HttpStatusCode.NotFound, "No se encontro aplicación!");
            }
            catch (Exception ex)
            {
                return new OperationResult(HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        public async Task<IOperationResultList<AplicacionDto>> GetAplicacionByTerm(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var aplicacion = _aplicacion.Search(x => (term == null || x.Nombre.Contains(term)))
                    .AsQueryable(); // Cargo el resto de la información requerida;

                return await aplicacion.ToResultListAsync<Aplicacion, AplicacionDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return new OperationResultList<AplicacionDto>(ex);
            }
        }

        public async Task<IOperationResultList<AplicacionDto>> GetAplicacionsOnboarding()
        {
            var result = _db.Aplicacion.Search(x => x.Onboarding == true)
                .Include(x => x.IdProductoNavigation);

            var res = _mapper.Map<List<AplicacionDto>>(result);

            return await res.ToResultListAsync();
        }

    }
}
