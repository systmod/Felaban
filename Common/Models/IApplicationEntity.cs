using System;

namespace Common
{
    public interface IApplicationEntity
    {
        public int IdAplicacion { get; set; }
        public string Nombre { get; set; }
        public TipoAplicacionEnum Tipo { get; set; }
        public Guid Token { get; set; }
        IProductEntity Producto { get; }
    }
}
