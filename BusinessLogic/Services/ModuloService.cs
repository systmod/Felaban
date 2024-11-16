using AutoMapper;
using Common;
using Common.Domain.Models;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using ConcentratorFraud.Felaban.Auth.Domain.Services;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
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
    public class ModuloService: IModuloService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<ModuloService> _logger;
        private readonly IEntityRepository<Modulo> _modulo;

        public ModuloService(
                IMapper mapper,
                IConfiguration appSettings,
                AuthContext db,
                IAuthContextProcedures proc,
                ILogger<ModuloService> logger,
                IEntityRepository<Modulo> modulo
            )
        {
            _mapper = mapper;
            _appSettings = appSettings;
            _procedures = proc;
            _db = db;
            _logger = logger;
            _modulo = modulo;
        }

        public async Task<IOperationResult<List<ModuloDto>>> GetModulos(IOperationRequest model)
        {
            var roles = _modulo.SearchAll(x => x.IdAplicacion == model.Aplicacion.IdAplicacion)
                                  .Select(x => _mapper.Map<ModuloDto>(x))
                                  .ToList();

            return await roles.ToResultAsync();
        }

        public async Task<IOperationResult<ModuloDto>> PostModulo(IOperationRequest<ModuloRequest> request)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(request.Data);
                await Task.WhenAll(
                    _modulo.InsertAsync(modulo),
                    _modulo.SaveAsync(request)
                    );

                var result = _mapper.Map<ModuloDto>(modulo);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<ModuloDto>();
            }
        }

        public async Task<IOperationResult<ModuloDto>> PutModulo(IOperationRequest<ModuloRequest> request, int id)
        {
            try
            {
                var modulo = _modulo.Search(x => x.IdModulo == id).FirstOrDefault();
                modulo = _mapper.Map(request.Data, modulo);

                await Task.WhenAll(
                    _modulo.UpdateAsync(modulo),
                    _modulo.SaveAsync(request)
                    );

                var result = _mapper.Map<ModuloDto>(modulo);

                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return await ex.ToResultAsync<ModuloDto>();
            }
        }

        public async Task<IOperationResult> DeleteModulo(IOperationRequest request, int id)
        {
            var modulo = _modulo.FirstOrDefault(x => x.IdEstado == 1 && x.IdModulo == id);

            if (modulo != null)
            {
                await _modulo.DeleteAsync(modulo);
                await _modulo.SaveAsync(request);

                return new OperationResult(HttpStatusCode.OK);
            }

            return new OperationResult(HttpStatusCode.NotFound, "No se encontró el Modulo!");
        }

    }
}
