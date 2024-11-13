using Common.Domain.Models;
using Common;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("roles")]
    [ApiController]    
    public class RolesController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public RolesController(
            IUserService userService
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
