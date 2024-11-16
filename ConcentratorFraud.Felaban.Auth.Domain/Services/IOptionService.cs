using Common.Domain.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcentratorFraud.Felaban.Auth.Domain.Request;

namespace ConcentratorFraud.Felaban.Auth.Domain.Services
{
    public interface IOptionService
    {
        Task<IOperationResult<List<OpcionDto>>> GetOpcionesByModulo(IOperationRequest model, int idModulo);
        Task<IOperationResult<OpcionDto>> PostOpcion(IOperationRequest<OpcionRequest> request);
        Task<IOperationResult<OpcionDto>> PutOpcion(IOperationRequest<OpcionRequest> request, int id);
        Task<IOperationResult> DeleteOpcion(IOperationRequest request, int id);
    }
}
