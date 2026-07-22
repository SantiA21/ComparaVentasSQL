using CinetCore.Data;
using CinetCore.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CinetCore.Services.ComparacionExcel
{
    public class VentasERPService
    {
        private readonly DataAccess _dataAccess;

        public VentasERPService(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public VentaResultado ObtenerLocalYExistencia(
            string dbKey,
            string sucCodigo,
            string numero,
            string tipo)
        {
            string query = @"
                SELECT TOP 1 PERI_CODIGO, vene_fecha
                FROM ventas_e 
                WHERE suc_codigo = @suc 
                  AND vene_numero = @num 
                  AND cbtee_codigo = @tipo;

                SELECT TOP 1 PERI_CODIGO 
                FROM ventas_e 
                WHERE suc_codigo = @suc;
            ";

            using var conn = _dataAccess.GetConnection(dbKey);
            using var multi = conn.QueryMultiple(query, new { suc = sucCodigo, num = numero, tipo = tipo });

            var firstResult = multi.ReadFirstOrDefault();
            if (firstResult != null && firstResult.PERI_CODIGO != null)
            {
                return new VentaResultado
                {
                    Local = firstResult.PERI_CODIGO.ToString(),
                    Existe = "✅ Existe",
                    Fecha = firstResult.vene_fecha != null
                             ? Convert.ToDateTime(firstResult.vene_fecha).ToString("dd/MM/yyyy")
                             : ""
                };
            }

            var secondResult = multi.ReadFirstOrDefault();
            if (secondResult != null && secondResult.PERI_CODIGO != null)
            {
                return new VentaResultado
                {
                    Local = secondResult.PERI_CODIGO.ToString(),
                    Existe = "❌ No existe"
                };
            }

            return new VentaResultado
            {
                Local = "-",
                Existe = "❌ No existe"
            };
        }
    }
}
