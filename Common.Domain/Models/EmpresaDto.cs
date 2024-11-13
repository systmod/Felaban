using System;

namespace Common.Domain.Models
{
    public partial class EmpresaDto : ICompanyEntity
    {
        public int IdEmpresa { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string CodigoSbs { get; set; }
        public string Pais { get; set; }
        public string Moneda { get; set; }
        public string UrlLogo { get; set; }
        public string Siglas { get; set; }
        public string Celular { get; set; }
        public string TokenWa { get; set; }
        public string Instancia { get; set; }
        public bool FacturaFanId { get; set; }
        public int IdLocalBodegaTransitoria { get; set; }
        public string? UTC { get; set; }
        public Guid Token { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }

    }
}
