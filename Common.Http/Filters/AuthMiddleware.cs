using Common.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Filters
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<AuthMiddleware> _logger;
        private readonly string _authAPI;
        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger, IConfiguration appSettings)
        {
            _next = next;
            _appSettings = appSettings;
            _logger = logger;
            _authAPI = appSettings["ServicesUrl:Auth"];
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var appid = context.Request.Headers["ApiKey"].FirstOrDefault() ?? context.Request.Query["apiKey"].FirstOrDefault();

                var userToken = context.Request.Headers["Authorization"].FirstOrDefault();

                userToken = userToken?.Split(" ")?.Last();

                using var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);
                client.DefaultRequestHeaders.Add("ApiKey", appid);

                var result = await client.PostAsync($"{_authAPI}/token", default);

                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<AuthResult>(data);

                    context.Items["User"] = user.Result;
                    context.Items["Status"] = user.Result.Status;
                    context.Items["Aplicacion"] = user.Result.Aplicacion;
                    context.Items["Empresa"] = user.Result.Empresa;
                    context.Items["Local"] = user.Result.Local;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AUTH: Error de Autenticación: {0}", ex.Message);
            }
            finally
            {
                await _next(context);
            }
        }
    }
}