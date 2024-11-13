using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Services
{
    public interface IPermissionsService
    {
        Task<IOperationResultList<OpcionDto>> GetOptions(IOperationRequest model, string? term = default, string? idPerfil = default, int page = 1, int pageSize = 10);
        Task<IOperationResult<List<ModuloDto>>> GetPermissions(IOperationRequest request, string userLogin, string localid);
        Task<IOperationResultList<ModuloDto>> GetModulesByIdApplication(IOperationRequest model, string? term = default, int page = 1, int pageSize = 10);
        Task<IOperationResultList<OpcionDto>> GetOptionsByIdModule(IOperationRequest model, int idModule, string? term = default, int page = 1, int pageSize = 10);
    }
}
