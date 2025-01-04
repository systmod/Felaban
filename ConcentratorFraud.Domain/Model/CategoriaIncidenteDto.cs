using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Domain.Model
{
    public class CategoriaIncidenteDto
    {
        public int IdCategoriaIncidente { get; set; }
        public int IdTipoIncidente { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
