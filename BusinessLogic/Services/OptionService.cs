using AutoMapper;
using Common;
using Common.Domain.Models;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using ConcentratorFraud.Felaban.Auth.Domain.Services;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Felaban.Auth.BusinessLogic.Services
{
    public class OptionService: IOptionService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<OptionService> _logger;
        private readonly IEntityRepository<Opcion> _opcion;

        public OptionService(
                IMapper mapper,
                IConfiguration appSettings,
                AuthContext db,
                IAuthContextProcedures proc,
                ILogger<OptionService> logger,
                IEntityRepository<Opcion> opcion
            )
        {
            _mapper = mapper;
            _appSettings = appSettings;
            _procedures = proc;
            _db = db;
            _logger = logger;
            _opcion = opcion;
        }

        public async Task<IOperationResult<List<OpcionDto>>> GetOpcionesByModulo(IOperationRequest model, int idModulo)
        {
            var roles = _opcion.SearchAll(x => x.IdModulo == idModulo)
                                  .Select(x => _mapper.Map<OpcionDto>(x))
                                  .ToList();

            return await roles.ToResultAsync();
        }

        public async Task<IOperationResult<OpcionDto>> PostOpcion(IOperationRequest<OpcionRequest> request)
        {
            try
            {
                var opcion = _mapper.Map<Opcion>(request.Data);
                await Task.WhenAll(
                    _opcion.InsertAsync(opcion),
                    _opcion.SaveAsync(request)
                    );

                var result = _mapper.Map<OpcionDto>(opcion);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<OpcionDto>();
            }
        }

        public async Task<IOperationResult<OpcionDto>> PutOpcion(IOperationRequest<OpcionRequest> request, int id)
        {
            try
            {
                var opcion = _opcion.Search(x => x.IdModulo == id).FirstOrDefault();
                opcion = _mapper.Map(request.Data, opcion);

                await Task.WhenAll(
                    _opcion.UpdateAsync(opcion),
                    _opcion.SaveAsync(request)
                    );

                var result = _mapper.Map<OpcionDto>(opcion);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<OpcionDto>();
            }
        }

        public async Task<IOperationResult> DeleteOpcion(IOperationRequest request, int id)
        {
            var opcion = _opcion.FirstOrDefault(x => x.IdEstado == 1 && x.IdModulo == id);

            if (opcion != null)
            {
                await _opcion.DeleteAsync(opcion);
                await _opcion.SaveAsync(request);

                return new OperationResult(HttpStatusCode.OK);
            }

            return new OperationResult(HttpStatusCode.NotFound, "No se encontró la opción!");
        }

    }
}
