using Common.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Domain.Services
{
    public interface IUserService
    {
        Task<IOperationResult<List<UnidadAdminDto>>> GetAdminUnitsByUserAndCompany(IOperationRequest<int> model);
        
        Task<IOperationResult<AuthenticationResponse>> Authenticate(LoginRequest model);

        Task<IOperationResult<AuthorizationResponse>> Authorize(IOperationRequest<AuthorizationRequest> request);

        Task<IOperationResult<UsuarioDto>> ChangePassword(string token, string password, bool habilitado = true);

        Task<IOperationResult<UsuarioDto>> ResetPassword(string username, int? idProducto);        

        Task<IOperationResult<UnidadAdminDto>> GetUnidadAdminByToken(string token);

        Task<IOperationResult<AplicacionDto>> GetAplicacionByToken(string token);        

        Task<IOperationResult<UsuarioDto>> GetByToken(string token);

        Task<IOperationResult<UsuarioDto>> GetByLogin(string login);

        Task<IOperationResult<UsuarioDto>> Register(IOperationRequest<UserRegisterRequest> request);

        Task<IOperationResult<UsuarioDto>> GetUserById(string id);

        Task<IOperationResult<UsuarioDto>> GetUserProfile(string userLogin, IOperationRequest model);
                

        Task<IOperationResult<List<UnidadAdminDto>>> GetAdminUnitsByUser(IOperationRequest<string> model);
        
        Task<IOperationResultList<UnidadAdminDto>> GetAdminUnitsByCompany(IOperationRequest model, string term, int page = 1, int pageSize = 10);
        
        Task<IOperationResult<UsuarioUnidadDto>> AssignAdminUnit(IOperationRequest<AssignUnitRequest> operationRequest);

        Task<IOperationResult<List<UnidadAdminDto>>> GetAdminUnitsByIdUser(IOperationRequest<string> idUser, string? term = default);

        Task<IOperationResult<UsuarioDto>> UpdateUser(string userLogin, IOperationRequest<UserUpdateRequest> request);

        Task<IOperationResultList<UsuarioDto>> GetAllUsers(IOperationRequest<FindUsersRequest> model, int page = 1, int? pageSize = 10);
        IOperationResult ValidateUsername(string username);

        IOperationResult CheckMail(string correo, int idAplicacion);
        Task<IOperationResult<string>> GetPlantillaByAplicacion(int idAplicacion);
        Task<IOperationResult<UserMailVerifyDto>> SaveToVerify(IOperationRequest<UserToVerifyRequest> request);
        Task<string> VerifyUserMail(long Id);
        Task<IOperationResult> PostLogAudit(IOperationRequest<AuditLogsRequest> request);
        

        #region Empresa
        Task<IOperationResult<EmpresaDto>> GetEmpresaByToken(string token);
        Task<IOperationResult<List<EmpresaDto>>> CompaniesByUser(IOperationRequest request);
        #endregion

        Task<IOperationResult<UsuarioDto>> PutUser(IOperationRequest<UsuarioRequest> request, string id);
        Task<IOperationResult> DeleteUsuario(IOperationRequest request, string id);
    }

}
