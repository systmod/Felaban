using Common.Domain.Models;
using Common;
using Common.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ConcentratorFraud.Felaban.Auth.Domain.Request;
using ConcentratorFraud.Felaban.Auth.Domain.Services;

namespace API.Controllers
{
    [Route("roles")]
    [ApiController]    
    public class RolesController : ApiControllerBase
    {
        private readonly IRolesService _userService;

        public RolesController(
            IRolesService userService
            )
        {
            _userService = userService;
        }

        [HttpGet("")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(IOperationResult<RolesDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _userService.GetRoles(this.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost, Route("crear")]
        [TokenAuthorize]
        [ProducesResponseType(typeof(IOperationResult<PerfilDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GuardarPerfil([FromBody] PerfilRequest request)
        {
            try
            {
                var result = await _userService.PostPerfil(request.ToRequest(this));

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
        [ProducesResponseType(typeof(IOperationResult<PerfilDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> ModificarPerfil([FromBody] PerfilRequest request, Guid id)
        {
            try
            {
                var result = await _userService.PutPerfil(request.ToRequest(this), id);

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
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var result = await _userService.DeletePerfil(this, id);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("{idAplicacion}/by-perfil")]
        [ProducesResponseType(typeof(IOperationResult<RolesDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetRolesByAplicacion(int idAplicacion)
        {
            var result = await _userService.GetRolesByAplicacion(this.ToRequest(this), idAplicacion);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("{id}/details")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(IOperationResult<OptionDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetDetailsByIdPerfil(string id)
        {
            var result = await _userService.GetDetailsByIdPerfil(id);

            if (result.Success)
            { 
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost("{id}/detail")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> AssignOptions(string id, List<ProfileDetailRequest> model)
        {
            var result = await _userService.AssignOptions(id, model.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpDelete("Detail/{id}")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> DeleteDetail(long id)
        {
            var result = await _userService.DeleteDetail(id, this);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
