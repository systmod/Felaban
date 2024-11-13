using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("login")]
    [ApiController]

    public class LoginController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<LoginController> _logger;

        public LoginController(IConfiguration config,
            IUserService userService,
            ILogger<LoginController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest model)
        {
            try
            {

                IActionResult response = StatusCode(401, new OperationResult(HttpStatusCode.Unauthorized, "No Autorizado"));
                var credentials = Request.Headers["Authorization"].DecodeBasicToken();

                if (credentials.Length > 1)
                {
                    var login = new LoginRequest
                    {
                        DeviceId = model.DeviceId,
                        AppId = model.AppId,
                        Latitud = model.Latitud,
                        Longitud = model.Longitud,

                        Usuario = credentials[0],
                        Clave = credentials[1]
                    };

                    var result = await _userService.Authenticate(login);

                    if (result.Success)
                        return Ok(result);

                    return StatusCode(result);
                }

                return response;
            }
            catch (Exception ex)
            {
                return StatusCode(401, new OperationResult(HttpStatusCode.Unauthorized, "No Autorizado", ex.ToString()));
            }
        }

    }
}
