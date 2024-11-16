using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class UsuarioRequest
    {
        public string Identificacion { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Foto { get; set; }
    }
}
