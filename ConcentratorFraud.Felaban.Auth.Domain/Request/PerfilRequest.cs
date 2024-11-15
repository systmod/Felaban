using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Felaban.Auth.Domain.Request
{
    public class PerfilRequest
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdTipoPerfil { get; set; }
        public int IdProducto { get; set; }
        public int IdAplicacion { get; set; }
    }
}
