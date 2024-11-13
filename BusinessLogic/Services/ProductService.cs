using AutoMapper;
using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ProductService: IProductService
    {
        private readonly AuthContext _db;
        private readonly IAuthContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Producto> _producto;
        public ProductService(
                            AuthContext db,
                IAuthContextProcedures proc,
                IMapper mapper,
                IEntityRepository<Producto> producto
            )
        {
            _db = db;
            _procedures = proc;
            _mapper = mapper;
            _producto = producto;
        }

        public async Task<IOperationResult<ProductoDto>> GuardarProducto(IOperationRequest<ProductoRequest> request)
        {
            try
            {
                var producto = _producto.Search(x => x.IdProducto == request.Data.IdProducto)
                            .FirstOrDefault();

                if (producto != null)
                {
                    producto = _mapper.Map(request.Data, producto);

                    await Task.WhenAll(
                    _producto.UpdateAsync(producto),
                    _producto.SaveAsync(request));
                }
                else
                {
                    var resultado = _producto.Search(x => x.Nombre == request.Data.Nombre)
                                .FirstOrDefault();

                    if (resultado != null)
                    {
                        return new OperationResult<ProductoDto>(HttpStatusCode.NotFound, "Ya existe nombre de producto");
                    }

                    producto = _mapper.Map<Producto>(request.Data);

                    await Task.WhenAll(
                    _producto.InsertAsync(producto),
                    _producto.SaveAsync(request));
                }

                var result = _mapper.Map<ProductoDto>(producto);
                return await result.ToResultAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult<ProductoDto>(ex);
            }
        }

        public async Task<IOperationResult> EliminarProducto(IOperationRequest request, int id)
        {
            try
            {
                var producto = await _producto.Search(x => x.IdProducto == id && x.IdEstado == 1)
                            .FirstOrDefaultAsync();

                if (producto != null)
                {
                    await _producto.DeleteAsync(producto);
                    await _producto.SaveAsync(request);

                    return new OperationResult(HttpStatusCode.OK);
                }

                return new OperationResult(HttpStatusCode.NotFound, "No se encontro producto!");
            }
            catch (Exception ex)
            {
                return new OperationResult(HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        public async Task<IOperationResultList<ProductoDto>> GetProductosByTerm(string term = default, int page = 1, int? pageSize = default)
        {
            try
            {
                var productos = _producto.Search(x => (term == null || x.Nombre.Contains(term)))
                    .AsQueryable(); // Cargo el resto de la información requerida;

                return await productos.ToResultListAsync<Producto, ProductoDto>(page, pageSize);
            }
            catch (Exception ex)
            {
                return new OperationResultList<ProductoDto>(ex);
            }
        }

    }
}
