using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class AplicacionRequest
    {
        public int? IdAplicacion { get; set; }
        public string Nombre { get; set; }
        public int IdProducto { get; set; }
        public Guid Token { get; set; }
        public short Tipo { get; set; }
        public bool Directo { get; set; }
        public int? NumeracionEmpresa { get; set; }
        public bool? Onboarding { get; set; }
        public string PlantillaCorreoVerificacion { get; set; }
        public bool IdEstado { get; set; }
    }
}
