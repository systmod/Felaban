using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("password")]
    [ApiController]

    public class PasswordController : ApiControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;
        private ILogger<LoginController> _logger;

        public PasswordController(IConfiguration config,
            IUserService userService,
            ILogger<LoginController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("")]
        //[TokenAuthorize(TokenPermissionEnum.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (Usuario != null)
            {
                var result = await _userService.ChangePassword(Usuario.Token, request?.Clave);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }

            return StatusCode(401, new OperationResult(HttpStatusCode.Unauthorized, "No Autorizado"));
        }


        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetPassword(request.Usuario, request.IdProducto);

            if (result.Success)
            {
                await Tools.SendMailAsync(result.Result, result.Result.Correo, "Se ha solicitado cambiar tu clave de acceso", "user_reset");

                return Ok(new ResetPasswordResponse { Token = result.Result.Token }.ToResult());
            }

            return BadRequest(result);
        }


        [HttpGet("confirm/{token}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ConfirmPassword(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.FromBase64String(); // Descomprimimos el mensaje
                    var data = token.Split(":");

                    // Validamos que cumple el formato requerido:
                    if (long.TryParse(data.Last(), out long time))
                    {
                        var exp = DateTime.FromBinary(time);

                        // Verificamos si la solicitud expiro:
                        if (exp >= DateTime.Now)
                        {
                            var usertoken = data.First(); // Usamos el token de seguridad
                            var pass = data.GeneratePassword();
                            var user = await _userService.ChangePassword(usertoken, pass, false);

                            if (user != null)
                            {
                                if (user.Success)
                                {
                                    user.Result.Token = pass;

                                    await Tools.SendMailAsync(user.Result, user.Result.Correo, "Se ha solicitado cambiar tu clave de acceso", "user_password");

                                    return HTML("user_email_sent", user.Result);
                                }
                                else
                                {
                                    return HTML("error_mensaje", new { mensaje = user.Message });
                                }
                            }

                            return HTML("error_mensaje", new { mensaje = "Hubo un error al recuperar su contraseña... Por favor intente mas tarde." });
                        }
                    }
                }

            }
            catch { }

            return HTML("error_mensaje", new { mensaje = "El codigo de confirmacion para resetear su clave de acceso ha caducado." });
        }

    }
}
