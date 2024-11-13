using System;
using System.Collections.Generic;

namespace Common
{
    public interface IUserEntity
    {
        string IdUsuario { get; }
        Guid IdPerfil { get; set; }
        string Nombre { get; }
        string Correo { get; }
        string InicioSesion { get; }
        string Token { get; set; }
        string Identificacion { get; }
        string Ruc { get; }

        public TokenPermissionEnum Status { get; }

        IAdminUnitEntity Local { get; }

        ICompanyEntity Empresa { get; }

        IApplicationEntity Aplicacion { get; }

        ICollection<string> Roles { get; }
    }
}
