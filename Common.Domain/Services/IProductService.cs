using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Services
{
    public interface IProductService
    {
        Task<IOperationResult<ProductoDto>> GuardarProducto(IOperationRequest<ProductoRequest> request);
        Task<IOperationResult> EliminarProducto(IOperationRequest request, int id);
        Task<IOperationResultList<ProductoDto>> GetProductosByTerm(string term = default, int page = 1, int? pageSize = default);
    }
}
