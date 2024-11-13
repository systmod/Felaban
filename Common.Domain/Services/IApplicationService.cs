using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Services
{
    public interface IApplicationService
    {
        Task<IOperationResult<AplicacionDto>> GuardarAplicacion(IOperationRequest<AplicacionRequest> request);
        Task<IOperationResult> EliminarAplicacion(IOperationRequest request, int id);
        Task<IOperationResultList<AplicacionDto>> GetAplicacionByTerm(string term = default, int page = 1, int? pageSize = default);
        Task<IOperationResultList<AplicacionDto>> GetAplicacionsOnboarding();

    }
}
