using Common;
using Common.Domain.Models;
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
    [Route("authorize")]
    [ApiController]

    public class AuthorizeController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<AuthorizeController> _logger;

        public AuthorizeController(IConfiguration config,
            IUserService userService,
            ILogger<AuthorizeController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [TokenAuthorize(TokenPermissionEnum.Login)]
        [HttpPost("{uid}")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> Authorize(string uid)
        {
            try
            {
                var model = new AuthorizationRequest { IdUnidadAdmin = uid };
                var response = await _userService.Authorize(model.ToRequest(this));

                if (response.Success)
                {
                    return response.ToObjectResult();
                }

                return StatusCode(response); ;
            }
            catch (Exception ex)
            {
                return StatusCode(401, new OperationResult(HttpStatusCode.Unauthorized, "No Autorizado", ex.ToString())); ;
            }
        }

    }
}
