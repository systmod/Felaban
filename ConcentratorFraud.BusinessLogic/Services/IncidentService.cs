using AutoMapper;
using Common;
using ConcentratorFraud.DataAccess.Models;
using ConcentratorFraud.Domain.Model;
using ConcentratorFraud.Domain.Request;
using ConcentratorFraud.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.BusinessLogic.Services
{
    public class IncidentService: IIncidentService
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Incidente> _incidente;
        public IncidentService(
            IMapper mapper,
            IEntityRepository<Incidente> incidente)
        {
            _mapper = mapper;
            _incidente = incidente;
        }

        public async Task<IOperationResult<IncidenteDto>> GuardarIncidente(IOperationRequest<IncidenteRequest> request)
        {
            try
            {
               
                var incidente = _mapper.Map<Incidente>(request.Data);

                await _incidente.InsertAsync(incidente);
                await _incidente.SaveAsync(request);

                var result = _mapper.Map<IncidenteDto>(incidente);
                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<IncidenteDto>(ex);
            }
        }
    }
}
