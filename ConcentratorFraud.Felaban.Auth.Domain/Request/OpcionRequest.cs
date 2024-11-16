using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Felaban.Auth.Domain.Request
{
    public class OpcionRequest
    {
        public int IdModulo { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Accion { get; set; }
        public string Operaciones { get; set; }
        public string Ayuda { get; set; }

    }
}
