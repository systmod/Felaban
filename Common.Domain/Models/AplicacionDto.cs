using System;

namespace Common.Domain.Models
{
    public class AplicacionDto : IApplicationEntity
    {
        public int IdAplicacion { get; set; }
        public string Nombre { get; set; }
        public Guid Token { get; set; }
        public TipoAplicacionEnum Tipo { get; set; }
        public bool Directo { get; set; }
        public bool IdEstado { get; set; }
        public int IdProducto { get; set; }

        public ProductoDto Producto { get; set; }

        IProductEntity IApplicationEntity.Producto { get => Producto; }
    }

}