using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Models
{
    public class EmpresaRequest
    {
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string CodigoSbs { get; set; }
        public string Pais { get; set; }
        public string Moneda { get; set; }        
        public string? Siglas { get; set; }        
        public string? Celular { get; set; }
        public string? TokenWa { get; set; }
        public string? Instancia { get; set; }
        public bool Aplicacion { get; set; }
        public bool FacturaFanId { get; set; }
        public int IdLocalBodegaTransitoria { get; set; }
        public string? UTC { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? Logo { get; set; }
        public int? IdEmpresaD { get; set; }
        public int? IdEmpresaH { get; set; }
    }

    public class EmpresaUnidadUserRequest
    {
        public Guid TokenAplicacion { get; set; }
        public Guid TokenPerfil { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string Celular { get; set; }
        public string UTC { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string InicioSesion { get; set; }
        public string PassWord { get; set; }
        public Guid? IdOrganizacion { get; set; }
    }		
}
