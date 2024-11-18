using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Domain.Models
{

    public class LoginRequest : AuthenticationRequest
    {
        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Clave { get; set; }
    }

    public class AuthorizationRequest
    {
        [Required]
        public string IdUnidadAdmin { get; set; }
    }

    public class AuthenticationRequest
    {
        [Required]
        public string AppId { get; set; }

        [Required]
        public string DeviceId { get; set; }

        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }

    public class ConfirmPasswordResponse
    {
        [Required]
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        [Required]
        public string Clave { get; set; }
    }

    public class ResetPasswordRequest
    {
        [Required]
        public string Usuario { get; set; }
        public int IdProducto { get; set; }
    }

    public class ChangeEmailRequest
    {
        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Correo { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string Clave { get; set; }
    }

    public class ResetPasswordResponse
    {
        public string Token { get; set; }
    }

    public class AssignUnitRequest
    {
        public string IdUsuario { get; set; }
        public Guid IdPerfil { get; set; }
        public int? IdUnidadAdmin { get; set; }
        public bool Activo { get; set; }

    }

    public class UserRegisterRequest
    {
        [Required]
        [StringLength(100)]
        public string InicioSesion { get; set; }
        [Required]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [StringLength(100)]
        public string Nombres { get; set; }
         
        [Required]
        [StringLength(500)]
        public string Correo { get; set; }

        public string Celular { get; set; }

        public string? Foto { get; set; } 
        public string? Clave { get; set; }

        public string IdPerfil { get; set; }

        public int? IdLocal { get; set; }
        public int? IdEmpresa { get; set; }
        public byte[]? GetBytes()
        {
            if (!string.IsNullOrEmpty(Foto))
            {
                var file = Foto?.Substring(Foto.IndexOf(",") + 1);
                var result = Convert.FromBase64String(file);
                return result;
            }

            return Array.Empty<byte>();
        }
        public string? MimeType => !string.IsNullOrEmpty(Foto) ? Foto?.Substring(0, Foto.IndexOf(",") + 1).Replace("data:", "") : "text/plain";

    }

    public class UserUpdateRequest
    {
        [Required]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(500)]
        public string Correo { get; set; }

        public string Celular { get; set; }

    }


    public class AuthorizationResponse : AuthenticationResponse
    {
        public ICompanyEntity Empresa { get; private set; }
        public IAdminUnitEntity Local { get; private set; }
        public AuthorizationResponse(IUserEntity user, string token)
            : base(user, token)
        {
            Empresa = user.Empresa;
            Local = user.Local;
        }
    }

    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Ruc { get; set; }
        public string Correo { get; set; }
        public string Alias { get; set; }
        public string Token { get; set; }

        public AuthenticationResponse(IUserEntity user, string token)
        {
            Id = user.Identificacion;
            Nombre = user.Nombre;
            Alias = user.InicioSesion;
            Correo = user.Correo;

            Token = token;
        }
    }

    public class RegisterDeviceRequest
    {
        [Required]
        public string Usuario { get; set; }
    }

    public class UserToVerifyRequest
    {
        public string IdUsuario { get; set; }
        public int IdAplicacion { get; set; }
        public string Correo { get; set; }
        public string Codigo { get; set; }
    }

}
