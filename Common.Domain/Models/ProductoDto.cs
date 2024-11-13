namespace Common.Domain.Models
{
    public class ProductoDto : IProductEntity
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string ApiURL { get; set; }
        public bool IdEstado { get; set; }
    }
}
