using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinetCore.Utils
{
    public static class IdUnicoParser
    {
        public static string NormalizarSucursal(string sucursal)
        {
            if (string.IsNullOrWhiteSpace(sucursal))
                return sucursal?.Trim() ?? "";

            string s = sucursal.Trim();
            return s.All(char.IsDigit) ? s.PadLeft(4, '0') : s;
        }

        public static string NormalizarComprobante(string comprobante)
        {
            if (string.IsNullOrWhiteSpace(comprobante))
                return comprobante?.Trim() ?? "";

            string c = comprobante.Trim();
            return c.All(char.IsDigit) ? c.PadLeft(8, '0') : c;
        }

        public static bool TryParse(
            string idUnico,
            out string sucursal,
            out string comprobante,
            out string tipo)
        {
            sucursal = "";
            comprobante = "";
            tipo = "";

            if (string.IsNullOrWhiteSpace(idUnico))
                return false;

            var partes = idUnico.Split('-');
            if (partes.Length != 3)
                return false;


            sucursal = NormalizarSucursal(partes[0]);


            comprobante = NormalizarComprobante(partes[1]);


            tipo = partes[2] switch
            {
                "1" => "FAA",
                "6" => "FAB",
                _ => "DESCONOCIDO"
            };

            return true;
        }
    }
}

