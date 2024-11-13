using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("profile")]
    [ApiController]

    public class ProfileController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<ProfileController> _logger;

        public ProfileController(IConfiguration config,
            IUserService userService,
            ILogger<ProfileController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [TokenAuthorize]
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(IOperationResult<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 400)]
        public async Task<IActionResult> GetUserById()
        {
            var userCode = Request.Headers["user-login"].FirstOrDefault() ?? Usuario.InicioSesion;

            var data = userCode.ToRequest(this);

            var result = await _userService.GetUserProfile(userCode, data);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [TokenAuthorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest request)
        {
            var userCode = Request.Headers["user-login"].FirstOrDefault();

            var result = await _userService.UpdateUser(userCode, request.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        

    }
}
