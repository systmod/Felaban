using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ConcentratorFraud.API
{
    public partial class Startup
    {
        public void ConfigureAuth(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Settings.GetValue<string>("SecretKey"));
            var issuer = Settings.GetValue<string>("JwtIssuer");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });



        }
    }
}
