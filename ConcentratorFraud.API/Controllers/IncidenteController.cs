using Common;
using Common.Domain.Services;
using Common.Http;
using ConcentratorFraud.Domain.Model;
using ConcentratorFraud.Domain.Request;
using ConcentratorFraud.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConcentratorFraud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidenteController : ApiControllerBase
    {
        private IConfiguration _config;
        private IIncidentService _incidente;
        private ILogger<IncidenteController> _logger;

        public IncidenteController(IConfiguration config,
            IIncidentService incidente,
            ILogger<IncidenteController> logger)
        {
            _config = config;
            _incidente = incidente;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IOperationResult<IncidenteDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> GuardarIncidente([FromBody] IncidenteRequest request)
        {
            try
            {
                var result = await _incidente.GuardarIncidente(request.ToRequest(this));

                if (result.Success)
                {
                    return Ok(result);
                }
                return result.ToObjectResult();

            }
            catch (Exception ex)
            {
                return ex.ToObjectResult();
            }
        }

    }
}
