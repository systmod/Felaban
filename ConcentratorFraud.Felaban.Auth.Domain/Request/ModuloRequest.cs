using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Felaban.Auth.Domain.Request
{
    public class ModuloRequest
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdAplicacion { get; set; }
        public string? Icono { get; set; }
        public int? Orden { get; set; }
    }
}
