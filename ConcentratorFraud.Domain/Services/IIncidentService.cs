using Common;
using ConcentratorFraud.Domain.Model;
using ConcentratorFraud.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Domain.Services
{
    public interface IIncidentService
    {
        Task<IOperationResult<IncidenteDto>> GuardarIncidente(IOperationRequest<IncidenteRequest> request);
    }
}
