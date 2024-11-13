using Common;
using Common.Domain.Models;
using Common.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NPOI.HSSF.Record.Chart;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Filters
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<JwtMiddleware> _logger;
        
        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, IConfiguration appSettings)
        {
            _next = next;
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {   
            var userToken = context.Request.GetBearerToken();

            if (userToken != null)
            {
                await AttachUserToContextAsync(context, userService, userToken);
            }
            else
            {
                var appToken = context.Request.GetApplicationToken();
                var companyToken = context.Request.GetCompanyToken();
                await AttachApplicationToContextAsync(context, userService, appToken, companyToken);
            }

            await _next(context);
        }

        // This helper method comes in 3.0. For now you'll need a copy in your app
        private static Endpoint GetEndpoint(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Features.Get<IEndpointFeature>()?.Endpoint;
        }

        private static bool IsAnonymous(HttpContext context)
        {
            var endpoint = GetEndpoint(context);
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>();

            return (allowAnonymous != null);
        }
        private async Task AttachApplicationToContextAsync(HttpContext context, IUserService userService, string appToken, string companyToken)
        {
            try
            {
                if (!IsAnonymous(context))
                {
                    var aplicacion = await userService.GetAplicacionByToken(appToken);
                    context.Items["Aplicacion"] = aplicacion.Result;

                    var company = await userService.GetEmpresaByToken(companyToken);
                    context.Items["Empresa"] = company.Result;
                }
            }
            catch (Exception ex)
            {
                var headers = JsonConvert.SerializeObject(context.Request.GetTypedHeaders());

                _logger.LogWarning(ex, "AUTH: Error de Autenticación: {0}\r\nApp Token: {1}\r\nPath: {2} {3}\r\nHeaders:\r\n\t{4}", ex.Message,
                        appToken,  
                        context.Request.Method,
                        context.Request.Path,
                        headers
                        );
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        private async Task AttachUserToContextAsync(HttpContext context, IUserService userService, string authToken)
        {
            try
            {
                if (!IsAnonymous(context))
                {
                    var jwtToken = ValidateToken(_appSettings["SecretKey"], authToken);

                    // Validamos si el usuario debe cambiar de contraseña
                    var statusValue     = jwtToken.Claims.FirstOrDefault(x => x.Type == IdentityClaims.Status)?.Value;
                    var userToken       = jwtToken.Claims.FirstOrDefault(x => x.Type == IdentityClaims.UserID)?.Value;
                    var appToken        = jwtToken.Claims.FirstOrDefault(x => x.Type == IdentityClaims.AppID)?.Value;
                    var companyToken    = jwtToken.Claims.FirstOrDefault(x => x.Type == IdentityClaims.CompanyID)?.Value;
                    var localToken      = jwtToken.Claims.FirstOrDefault(x => x.Type == IdentityClaims.LocalID)?.Value;

                    Enum.TryParse(statusValue, out TokenPermissionEnum status);

                    context.Items["Status"] = status;

                    var user = await userService.GetByToken(userToken);
                    

                    if (user.Success)
                    {
                        bool hasAtribute = jwtToken.Payload.ContainsKey("binasystem://idperfil");
                        if (hasAtribute)
                        {
                            string idperfil = jwtToken.Payload["binasystem://idperfil"].ToString();
                            user.Result.IdPerfil = new Guid(idperfil);
                        }
                        user.Result.Status = status;

                        // attach user to context on successful jwt validation
                        context.Items["User"] = user.Result;

                        var aplicacion = await userService.GetAplicacionByToken(appToken);
                        context.Items["Aplicacion"] = user.Result.Aplicacion = aplicacion.Result;

                        var empresa = await userService.GetEmpresaByToken(companyToken);
                        context.Items["Empresa"] = user.Result.Empresa = empresa.Result;

                        var local = await userService.GetUnidadAdminByToken(localToken);
                        context.Items["Local"] = user.Result.Local = local.Result;

                    }
                }
            }
            catch (Exception ex)
            {
                var headers = JsonConvert.SerializeObject(context.Request.GetTypedHeaders());

                _logger.LogWarning(ex, "AUTH: Error de Autenticación: {0}\r\nToken: {1}\r\nPath: {2} {3}\r\nHeaders:\r\n\t{4}", ex.Message,
                        authToken,
                        context.Request.Method,
                        context.Request.Path,
                        headers
                        );
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        public static JwtSecurityToken ValidateToken(string secretKey, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = (key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken as JwtSecurityToken;

        }
    }
}