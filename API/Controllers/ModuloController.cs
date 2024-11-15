using Common.Domain.Models;
using Common;
using Common.Http;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using ConcentratorFraud.Felaban.Auth.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace ConcentratorFraud.Felaban.Auth.API.Controllers
{
    [Route("modulo")]
    [ApiController]
    public class ModuloController : ApiControllerBase
    {
        private readonly IModuloService _userService;

        public ModuloController(
            IModuloService userService
            )
        {
            _userService = userService;
        }

        [HttpGet("")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(IOperationResult<ModuloDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetModulos()
        {
            var result = await _userService.GetModulos(this.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost, Route("crear")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<ModuloDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> PostModulo([FromBody] ModuloRequest request)
        {
            try
            {
                var result = await _userService.PostModulo(request.ToRequest(this));

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

        [HttpPut, Route("modificar/{id}")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<ModuloDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> PutModulo([FromBody] ModuloRequest request, int id)
        {
            try
            {
                var result = await _userService.PutModulo(request.ToRequest(this), id);

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

        [HttpDelete("borrar/{id}")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> DeleteModulo(int id)
        {
            var result = await _userService.DeleteModulo(this, id);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
