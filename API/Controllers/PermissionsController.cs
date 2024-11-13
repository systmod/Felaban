using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("permissions")]
    [ApiController]
    [TokenAuthorize(TokenPermissionEnum.Authorized)]
    public class PermissionsController : ApiControllerBase
    {
        private IConfiguration _config;
        private IPermissionsService _userService;
        private ILogger<PermissionsController> _logger;

        public PermissionsController(IConfiguration config,
            IPermissionsService userService,
            ILogger<PermissionsController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetPermissions()
        {
            var userCode = Request.Headers["user-login"].FirstOrDefault() ?? Usuario.InicioSesion;
            var localId = Request.Headers["local-id"].FirstOrDefault() ?? Local.Token.ToString();

            var result = await _userService.GetPermissions(this, userCode, localId);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("options")]
        [ProducesResponseType(typeof(IOperationResult<OptionDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetOptions(string? term = null, string? idPerfil = null, int page = 1, int pageSize = 10)
        {
            var result = await _userService.GetOptions(this, term, idPerfil, page, pageSize);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("modules/by-idapplication")]
        [ProducesResponseType(typeof(IOperationResult<ModuloDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetModulesByIdApplication(string? term = null, int page = 1, int pageSize = 10)
        {
            var result = await _userService.GetModulesByIdApplication(this, term, page, pageSize);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("options/by-idmodule/{id}")]
        [ProducesResponseType(typeof(IOperationResult<ModuloDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetOptionsByIdModule(int id,string? term = null, int page = 1, int pageSize = 10)
        {
            var result = await _userService.GetOptionsByIdModule(this, id, term, page, pageSize);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
