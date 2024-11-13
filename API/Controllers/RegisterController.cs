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
    [Route("register")]
    [ApiController]

    public class RegisterController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<RegisterController> _logger;

        public RegisterController(IConfiguration config,
            IUserService userService,
            ILogger<RegisterController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("")]
        [ApiAuthorize]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await _userService.Register(request.ToRequest(this));

            if (result.Success)
            {
                if (result.Result.Aplicacion.IdAplicacion == 15)
                {
                    await Tools.SendMailAsyncFc(result.Result, result.Result.Correo, "Bienvenido!", "registro.html");
                }
                if (result.Result.Aplicacion.IdAplicacion == 17)
                {
                    await Tools.SendMailAsyncFc(result.Result, result.Result.Correo, "Bienvenido!", "cupiRegister.html");
                }
                else
                {
                    await Tools.SendMailAsync(result.Result, result.Result.Correo, "Bienvenido!", "registro - SB.html");
                }
                return Ok(result);
            }

            return ((IOperationResult)result).ToObjectResult();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{Id}/verify")]
        [ApiAuthorize]
        [ProducesResponseType(typeof(IOperationResult<UserMailVerifyDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 404)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> VerifyUserMail(long Id)
        {
            var result = await _userService.VerifyUserMail(Id);
            return Ok(result);
        }


    }
}
