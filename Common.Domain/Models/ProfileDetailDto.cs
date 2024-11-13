using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class ProfileDetailDto
    {
        public long IdDetallePerfil { get; set; }
        public Guid IdPerfil { get; set; }
        public long IdOpcion { get; set; }
        public string Opcion { get; set; }
        public string Permisos { get; set; }
    }
}
