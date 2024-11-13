using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Services
{
    public interface ICompanyService
    {
        Task<IOperationResult> SaveCompany(IOperationRequest<EmpresaRequest> model);
        Task<IOperationResult<EmpresaDto>> UpdateEmpresa(IOperationRequest<EmpresaRequest> request);
        Task<IOperationResult<EmpresaDto>> GetEmpresaById(int id);
    }
}
