namespace CinetCore.Models
{
    public class ModificacionImporteRequest
    {
        public string Sucursal { get; set; }
        public string Comprobante { get; set; }
        public string TipoComprobante { get; set; }
        public decimal Total { get; set; }
        public decimal Iva { get; set; }
        public decimal Neto { get; set; }
    }
}
