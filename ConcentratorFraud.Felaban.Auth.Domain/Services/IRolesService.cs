using Common.Domain.Models;
using Common;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Felaban.Auth.Domain.Services
{
    public interface IRolesService
    {
        Task<IOperationResult<List<RolesDto>>> GetRoles(IOperationRequest model);
        Task<IOperationResult<List<RolesDto>>> GetRolesByAplicacion(IOperationRequest model, int idAplicacion);
        Task<IOperationResult<List<ProfileDetailDto>>> GetDetailsByIdPerfil(string idPerfil);
        Task<IOperationResultList<ProfileDetailDto>> AssignOptions(string idPerfil, IOperationRequest<List<ProfileDetailRequest>> request);
        Task<IOperationResult> DeleteDetail(long id, IOperationRequest request);
        Task<IOperationResult<PerfilDto>> PostPerfil(IOperationRequest<PerfilRequest> request);
        Task<IOperationResult<PerfilDto>> PutPerfil(IOperationRequest<PerfilRequest> request, Guid id);
        Task<IOperationResult> DeletePerfil(IOperationRequest request, Guid id);
        Task<IOperationResult<List<TipoPerfilDto>>> GetTipoPerfil(IOperationRequest model);
    }
}
