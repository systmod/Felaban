using Common.Domain.Models;
using Common;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace API.Controllers
{
    [Route("audit")]
    [ApiController]
    public class AuditController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<AuditController> _logger;

        public AuditController(IConfiguration config,
            IUserService userService,
            ILogger<AuditController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("save")]
        [ProducesResponseType(typeof(IOperationResult), 201)]
        [ProducesResponseType(typeof(IOperationResult), 404)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> PostLogAudit([FromBody] AuditLogsRequest request)
        {
            try
            {
                await _userService.PostLogAudit(request.ToRequest(this));              
                return Ok();              
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.ToObjectResult());
            }
        }

    }
}
