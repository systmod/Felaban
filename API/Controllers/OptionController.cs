using Common.Domain.Models;
using Common;
using Common.Http;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using ConcentratorFraud.Felaban.Auth.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccess.Models;

namespace ConcentratorFraud.Felaban.Auth.API.Controllers
{
    [Route("option")]
    [ApiController]
    public class OptionController : ApiControllerBase
    {
        private readonly IOptionService _userService;

        public OptionController(
            IOptionService userService
            )
        {
            _userService = userService;
        }

        [HttpGet("{idModulo}")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(IOperationResult<OpcionDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetOpcionesByModulo(int idModulo)
        {
            var result = await _userService.GetOpcionesByModulo(this.ToRequest(this), idModulo);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost, Route("crear")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<OpcionDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> PostOpcion([FromBody] OpcionRequest request)
        {
            try
            {
                var result = await _userService.PostOpcion(request.ToRequest(this));

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
        [ProducesResponseType(typeof(IOperationResult<OpcionDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> PutModulo([FromBody] OpcionRequest request, int id)
        {
            try
            {
                var result = await _userService.PutOpcion(request.ToRequest(this), id);

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
            var result = await _userService.DeleteOpcion(this, id);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
