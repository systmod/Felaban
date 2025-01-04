using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcentratorFraud.Domain.Model
{
    public class IncidenteDto
    {
        public int IdIncidente { get; set; }
        public int? IdTipoIncidente { get; set; }
        public int? IdCategoriaIncidente { get; set; }
        public int? IdPaisInstitucion { get; set; }
        public int? IdPaisLocalizacion { get; set; }
        public int? IdCiudadLocalizacion { get; set; }
        public DateTime? FechaHora { get; set; }
        public int? IdModoIncidente { get; set; }
        public string VulnerabilidadCve { get; set; }
        public int? IdNivelCriticidad { get; set; }
        public int? IdNivelImpactoActual { get; set; }
        public int? IdNivelImpactoEsperado { get; set; }
        public decimal? NivelPrioridad { get; set; }
        public int? IdDesignacionTlp { get; set; }
        public string Descripcion { get; set; }
        public string Referencias { get; set; }
    }
}
