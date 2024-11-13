using System;

namespace Common.Domain.Models
{
    public class UnidadAdminDto : IAdminUnitEntity
    {
        public int IdUnidadAdmin { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoUnidadAdmin { get; set; }  
        public string TipoUnidad { get; set; }
        public Guid Token { get; set; }
        public string? Direccion { get; set; }
        public string? Establecimiento { get; set; }
        public string? PuntoEmision { get; set; }
    }

    public class UsuarioUnidadDto
    {
        public int IdEmpresa { get; set; }

        public Guid IdPerfil { get; set; }
        public string Perfil { get; set; }

        public int? IdUnidadAdmin { get; set; }
        public string Descripcion { get; set; }      

        public bool Activo { get; set; }
        public string TipoUnidad { get; set; }
    }
}
