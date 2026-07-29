using System;
using System.Collections.Generic;

namespace CinetCore.Services.Salvaventas
{
    public class VentaRescueRequest
    {
        private string _sucCodigo;
        public string SucCodigo
        {
            get => _sucCodigo;
            set => _sucCodigo = CinetCore.Utils.IdUnicoParser.NormalizarSucursal(value);
        }

        private string _veneNumero;
        public string VeneNumero
        {
            get => _veneNumero;
            set => _veneNumero = CinetCore.Utils.IdUnicoParser.NormalizarComprobante(value);
        }
        public string CbteeCodigo { get; set; } = "FAB";
        public string ValCodigo { get; set; } = "EFECTIVO";
        public string Local { get; set; }
        public string Equipo { get; set; }
        public string Estado { get; set; } = "Pendiente...";
        public List<ResultGroup> ResultadosRescate { get; set; } = new List<ResultGroup>();
        public bool YaExiste { get; set; } = false;
        public bool Rescatable { get; set; } = false;

        public decimal? Importe { get; set; }
        public string CAE { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
