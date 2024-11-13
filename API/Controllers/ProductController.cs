using Common;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Common.Domain.Models;
using System.Collections.Generic;
using System.Numerics;
//using BusinessLogic.Services;

namespace API.Controllers
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {
        private IConfiguration _config;
        private IProductService _productoService;
        private ILogger<ProductController> _logger;

        public ProductController(IConfiguration config,
            IProductService productoService,
            ILogger<ProductController> logger)
        {
            _config = config;
            _productoService = productoService;
            _logger = logger;
        }

        [HttpPost, Route("crear")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<ProductoDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GuardarProducto([FromBody] ProductoRequest request)
        {
            try
            {
                var result = await _productoService.GuardarProducto(request.ToRequest(this));

                if (result.Success)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpDelete, Route("{id}")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<ProductoDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                var result = await _productoService.EliminarProducto(this, id);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return result.ToObjectResult();
                }
            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

        [HttpGet]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IEnumerable<ProductoDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetPlanes(string term = null, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _productoService.GetProductosByTerm(term, page, pageSize);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return result.ToObjectResult();
                }
            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

    }
}
