using System;

namespace Common
{
    public interface ICompanyEntity
    {
        int IdEmpresa { get; set; }
        string Ruc { get; set; }
        string RazonSocial { get; set; }
        string NombreComercial { get; set; }
        public string CodigoSbs { get; set; }
        public string Pais { get; set; }
        public string Moneda { get; set; }
        public string UrlLogo { get; set; }
        public string Siglas { get; set; }
        public string Celular { get; set; }
        public string TokenWa { get; set; }
        public string Instancia { get; set; }
        public Guid Token { get; set; }
        public bool FacturaFanId { get; set; }
        public int IdLocalBodegaTransitoria { get; set; }
        public string UTC { get; set; }


    }
}
