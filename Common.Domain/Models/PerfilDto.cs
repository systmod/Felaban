using System;
using System.Collections.Generic;

namespace Common.Domain.Models
{

    public class PerfilDto
    {
        public Guid IdPerfil { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public TipoPerfilDto TipoPerfil { get; set; }
        //public int IdProducto { get; set; }
        //public int IdAplicacion { get; set; }
        //public int IdEmpresa { get; set; }
        public bool Activo { get; set; }

        public List<ModuloDto> Permisos { get; set; }
    }
        
    public class OpcionDto : IOptionEntity
    {
        public long IdOpcion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Ayuda { get; set; } 

        public string Accion { get; set; }
        public string Operaciones { get; set; }
        public string Permisos { get; set; }
        public int Orden { get; set; }
    }

    public class ModuloDto : IModuleEntity
    {
        public int IdModulo { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public int? Orden { get; set; }

        public List<IOptionEntity> Opciones { get; set; }

    }

    public class TipoPerfilDto 
    {
        public int IdTipoPerfil { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool AccesoTotal { get; set; }
        public bool EsAdicional { get; set; }
    }
}
