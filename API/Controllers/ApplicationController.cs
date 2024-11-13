//using BusinessLogic.Services;
using Common.Domain.Models;
using Common;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("Application")]
    [ApiController]
    public class ApplicationController : ApiControllerBase
    {
        private IConfiguration _config;
        private IApplicationService _userService;
        private ILogger<AdminUnitsController> _logger;

        public ApplicationController(IConfiguration config,
            IApplicationService userService,
            ILogger<AdminUnitsController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost, Route("crear")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<AplicacionDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GuardarProducto([FromBody] AplicacionRequest request)
        {
            try
            {
                var result = await _userService.GuardarAplicacion(request.ToRequest(this));

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
        [ProducesResponseType(typeof(IOperationResult<AplicacionDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> EliminarAplicacion(int id)
        {
            try
            {
                var result = await _userService.EliminarAplicacion(this, id);

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
        [ProducesResponseType(typeof(IEnumerable<AplicacionDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetAplicacionByTerm(string term = null, int page = 1, int? pageSize = default)
        {
            try
            {
                var result = await _userService.GetAplicacionByTerm(term, page, pageSize);

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

        [HttpGet("get/onboarding")]
        public async Task<IActionResult> GetAplicacionsOnboarding()
        {
            var result = await _userService.GetAplicacionsOnboarding();

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
