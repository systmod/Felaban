using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class ProductoRequest
    {
        public int? IdProducto { get; set; }
        public string Nombre { get; set; }
        public string ApiURL { get; set; }
        public bool IdEstado { get; set; }
    }
}
