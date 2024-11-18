using System;
using System.Collections.Generic;

namespace Common.Domain.Models
{
    public class UsuarioDto : IUserEntity
    {
        public UsuarioDto()
        {
            IdUsuario = "0";
            IdPerfil = Guid.Empty;
            Identificacion = "0000000000";
            Ruc = "N/A";
            Nombre = "SYSTEM USER";
            Correo = "app@binasystem.com";
            InicioSesion = "SYSTEM";
            Token = $"{Guid.Empty}";
            Roles = Array.Empty<string>();
        }

        public UsuarioDto(IUserEntity usuario, AplicacionDto app, EmpresaDto company, UnidadAdminDto local)
        {
            IdUsuario = usuario.IdUsuario;
            IdPerfil = usuario.IdPerfil;
            Identificacion = usuario.Identificacion;
            Ruc = usuario.Ruc;
            Nombre = usuario.Nombre;
            Correo = usuario.Correo;
            InicioSesion = usuario.InicioSesion;
            Token = usuario.Token;
            Roles = usuario.Roles;
            Status = usuario.Status;
            Aplicacion = app;
            Empresa = company;
            Local = local;
        }

        public string IdUsuario { get; set; }
        public Guid IdPerfil { get; set; }
        public string InicioSesion { get; set; }
        public string Identificacion { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Token { get; set; }
        public string Hash { get; set; }

        public TokenPermissionEnum Status { get; set; }

        public ICollection<string> Roles { get; set; }
        public PerfilDto Perfil { get; set; }

        public AplicacionDto Aplicacion { get; set; }

        IApplicationEntity IUserEntity.Aplicacion => this.Aplicacion;

        public EmpresaDto Empresa { get; set; }

        ICompanyEntity IUserEntity.Empresa => this.Empresa;

        public UnidadAdminDto Local { get; set; }

        IAdminUnitEntity IUserEntity.Local => this.Local;
    }

    public class UserMailVerifyDto
    {
        public long Id { get; set; }
        public string Correo { get; set; }
        public int IdAplicacion { get; set; }
        public string Codigo { get; set; }
        public bool Verificado { get; set; }
    }
}
