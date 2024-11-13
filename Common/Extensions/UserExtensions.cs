using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Common
{
    public static class UserExtensions
    {
        public static ClaimsIdentity ToIdentity(this IUserEntity user, TokenPermissionEnum status)
        {
            var claims = new List<Claim>(new[] {
                new Claim(IdentityClaims.AppID, $"{user.Aplicacion.Token}"),
                new Claim(IdentityClaims.UserID, user.Token),
                new Claim(IdentityClaims.Status, $"{status.GetValue()}"),
            });

            if (status == TokenPermissionEnum.Authorized)
            {
                claims.AddRange(new[] {
                        new Claim(IdentityClaims.IdentityID, $"{user.Identificacion}"),
                        new Claim(IdentityClaims.Ruc, $"{user.Ruc}"),
                        new Claim(IdentityClaims.CompanyID, $"{user.Empresa?.Token}"),
                        new Claim(IdentityClaims.LocalID, $"{user.Local?.Token}"),
                        new Claim(IdentityClaims.Name, user.InicioSesion),
                        new Claim(IdentityClaims.Fullname, user.Nombre),
                        new Claim(IdentityClaims.Email, user.Correo),
                        new Claim(IdentityClaims.Celular, $"{user.Empresa.Celular}"),
                        new Claim(IdentityClaims.TokenWa, $"{user.Empresa.TokenWa}"),
                        new Claim(IdentityClaims.Instancia, $"{user.Empresa.Instancia}"),
                        new Claim(IdentityClaims.FacturaFanId, $"{user.Empresa.FacturaFanId}"),
                        new Claim(IdentityClaims.IdLocalBodegaTransitoria, $"{user.Empresa.IdLocalBodegaTransitoria}"),
                        new Claim(IdentityClaims.Rol, $"{user.Roles.FirstOrDefault()}"),
                        new Claim(IdentityClaims.Utc, $"{user.Empresa.UTC}"),
                        new Claim(IdentityClaims.IdPerfil, $"{user.IdPerfil}"),
                });
            }

            return new ClaimsIdentity(claims);
        }

        public static string GenerateJwtToken(this IUserEntity user, string secretKey, TokenPermissionEnum status = TokenPermissionEnum.Login)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "binasystem.com",
                Subject = user.ToIdentity(status),
                Expires = DateTime.UtcNow.AddHours(8), // Tiempo de expiracion
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }


    public static class IdentityClaims
    {
        public const string UserID = "binasystem://userid";
        public const string Ruc = "binasystem://ruc";
        public const string Status = "binasystem://status";
        public const string AppID = "binasystem://appid";
        public const string CompanyID = "binasystem://companyid";
        public const string LocalID = "binasystem://localid";
        public const string IdPerfil = "binasystem://idperfil";

        public const string IdentityID = "binasystem://id";
        public const string Name = "binasystem://username";
        public const string Fullname = "binasystem://fullname";
        public const string Email = "binasystem://email";
        public const string Celular = "binasystem://celular";
        public const string Rol = "binasystem://rol";
        public const string TokenWa = "binasystem://tokenwa";
        public const string Instancia = "binasystem://instancia";
        public const string FacturaFanId = "binasystem://facturafanid";
        public const string IdLocalBodegaTransitoria = "binasystem://idlocalbodegatransitoria";
        public const string Utc = "binasystem://utc";
    }
}
