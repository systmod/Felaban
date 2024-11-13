#nullable enable
using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("admin-units")]
    [ApiController]
    [TokenAuthorize()]
    public class AdminUnitsController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<AdminUnitsController> _logger;

        public AdminUnitsController(IConfiguration config,
            IUserService userService,
            ILogger<AdminUnitsController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }


        [TokenAuthorize(TokenPermissionEnum.Authorized)]
        [HttpGet("company/{id}")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetAdminUnitsByUserAndCompany(int id = default)
        {
            var result = await _userService.GetAdminUnitsByUserAndCompany(id.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        /// <summary>
        /// Get de todos los locales de todas las empresas por l
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetAdminUnitsByUser(string? type = default)
        {
            var result = await _userService.GetAdminUnitsByUser(type.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetAdminUnitsByCompany(string term, int page= 1, int pageSize = 10)
        {
            var result = await _userService.GetAdminUnitsByCompany(this, term, page, pageSize);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost("assign")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> AssignAdminUnit([FromBody] AssignUnitRequest model)
        {
            var result = await _userService.AssignAdminUnit(model.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GetAdminUnitsByIdUser(string id, string? term = default)
        {
            var result = await _userService.GetAdminUnitsByIdUser(id.ToRequest(this), term);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
