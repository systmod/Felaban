using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<UsersController> _logger;

        public UsersController(IConfiguration config,
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [TokenAuthorize(TokenPermissionEnum.Authorized)]
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(IOperationResult<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 400)]
        public async Task<IActionResult> GetAllUsers(string term, Guid? rol = null, Guid? unit = null)
        {

            var model = new FindUsersRequest
            {
                Perfil = rol,
                Term = term,
                Unidad = unit
            };

            var result = await _userService.GetAllUsers(model.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [TokenAuthorize(TokenPermissionEnum.Authorized)]
        [HttpGet, Route("validate/{username}")]
        [ProducesResponseType(typeof(IOperationResult), 200)]
        [ProducesResponseType(typeof(IOperationResult), 400)]
        public IActionResult ValidateUsername(string username)
        {

            var result = _userService.ValidateUsername(username);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }


        [TokenAuthorize(TokenPermissionEnum.Login)]
        [HttpGet, Route("companies")]
        [ProducesResponseType(typeof(IOperationResult), 200)]
        [ProducesResponseType(typeof(IOperationResult), 400)]
        public async Task<IActionResult> CompaniesByUser()
        {
            var result = await _userService.CompaniesByUser(this.ToRequest(this));

            if (result.Success)
            {
                return Ok(result);
            }

            return result.ToObjectResult();
        }

        [HttpGet, Route("verify/{correo}/{idAplicacion}")]
        [ProducesResponseType(typeof(IOperationResult), 200)]
        [ProducesResponseType(typeof(IOperationResult), 400)]
        public IActionResult CheckMail(string correo, int idAplicacion)
        {

            var result = _userService.CheckMail(correo, idAplicacion);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }


        [HttpPost("verify")]
        [ProducesResponseType(typeof(IOperationResult<UserMailVerifyDto>), 201)]
        [ProducesResponseType(typeof(IOperationResult), 404)]
        [ProducesResponseType(typeof(IOperationResult), 500)]
        public async Task<IActionResult> RegisterToVerify([FromBody] UserToVerifyRequest request)
        {
            try
            {
                var result = await _userService.SaveToVerify(request.ToRequest(this));
                if (result.Success)
                {
                    var app = await _userService.GetPlantillaByAplicacion(request.IdAplicacion);

                    await Tools.SendMailAsync(result.Result, result.Result.Correo, "Bienvenido!", app.Result );
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.ToObjectResult());
            }
        }

    }
}
