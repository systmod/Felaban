using Common;
using Common.Domain.Models;
using ConcentratorFraud.Felaban.Auth.Domain.Request;

namespace ConcentratorFraud.Felaban.Auth.Domain.Services
{
    public interface IModuloService
    {
        Task<IOperationResult<List<ModuloDto>>> GetModulos(IOperationRequest model);
        Task<IOperationResult<ModuloDto>> PostModulo(IOperationRequest<ModuloRequest> request);
        Task<IOperationResult<ModuloDto>> PutModulo(IOperationRequest<ModuloRequest> request, int id);
        Task<IOperationResult> DeleteModulo(IOperationRequest request, int id);
    }
}
