using Common;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("token")]
    [ApiController]

    public class TokenController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<LoginController> _logger;

        public TokenController(IConfiguration config,
            IUserService userService,
            ILogger<LoginController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }


        [HttpPost("")]
        [TokenAuthorize()]
        public async Task<IActionResult> ValidateToken()
        {
            if (Usuario != null)
            {
                return Ok(await Usuario.ToResultAsync());
            }

            return StatusCode(401, new OperationResult(HttpStatusCode.Unauthorized, "No Autorizado"));
        }
    }
}