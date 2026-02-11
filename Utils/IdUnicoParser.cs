using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinetCore.Utils
{
    public static class IdUnicoParser
    {
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

            // Sucursal → 4 dígitos
            sucursal = partes[0].PadLeft(4, '0');

            // Comprobante → 8 dígitos
            comprobante = partes[1].PadLeft(8, '0');

            // Tipo
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

