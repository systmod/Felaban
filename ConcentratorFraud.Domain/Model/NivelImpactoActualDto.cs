using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Domain.Model
{
    public class NivelImpactoActualDto
    {
        public int IdNivelImpactoActual { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
