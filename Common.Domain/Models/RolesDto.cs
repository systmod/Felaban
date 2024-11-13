using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class RolesDto
    {
        public string IdPerfil { get; set; }
        public int IdProducto { get; set; }
        public int IdEmpresa { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool AccesoTotal { get; set; }
        public bool EsAdicional { get; set; }
    }
}
