using ComparaVentasExcel.Data;
using ComparaVentasExcel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparaVentasExcel.Services.ComparacionExcel
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
                SELECT TOP 1 PERI_CODIGO 
                FROM ventas_e 
                WHERE suc_codigo = @suc 
                  AND vene_numero = @num 
                  AND cbtee_codigo = @tipo;

                SELECT TOP 1 PERI_CODIGO 
                FROM ventas_e 
                WHERE suc_codigo = @suc;
            ";

            using var conn = _dataAccess.GetConnection(dbKey);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@suc", sucCodigo);
            cmd.Parameters.AddWithValue("@num", numero);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            // 1️⃣ Match exacto
            if (reader.Read() && reader[0] != DBNull.Value)
            {
                return new VentaResultado
                {
                    Local = reader[0].ToString(),
                    Existe = "✅ Existe"
                };
            }

            // 2️⃣ Fallback por sucursal
            if (reader.NextResult() && reader.Read() && reader[0] != DBNull.Value)
            {
                return new VentaResultado
                {
                    Local = reader[0].ToString(),
                    Existe = "❌ No existe"
                };
            }

            // 3️⃣ No hay nada
            return new VentaResultado
            {
                Local = "-",
                Existe = "❌ No existe"
            };
        }
    }
}
