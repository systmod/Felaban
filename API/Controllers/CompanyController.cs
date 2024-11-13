using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Company")]
    [ApiController]    
    public class CompanyController : ApiControllerBase
    {
        private IConfiguration _config;
        private ICompanyService _userService;
        private ILogger<AdminUnitsController> _logger;

        public CompanyController(IConfiguration config,
            ICompanyService userService,
            ILogger<AdminUnitsController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("get/{id}")]
        [TokenAuthorize()]
        public async Task<IActionResult> GetEmpresaById(int id)
        {
            var result = await _userService.GetEmpresaById(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPut("update")]
        [TokenAuthorize()]
        public async Task<IActionResult> UpdateEmpresa([FromBody] EmpresaRequest request)
        {
            var result = await _userService.UpdateEmpresa(request.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost("save")]
        [TokenAuthorize()]
        [ProducesResponseType(typeof(IOperationResult), 201)]
        [ProducesResponseType(typeof(IOperationResult), 404)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> SaveCompany([FromBody] EmpresaRequest request)
        {
            try
            {
                await _userService.SaveCompany(request.ToRequest(this));
                return Ok();
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.ToObjectResult());
            }
        }

    }
}
