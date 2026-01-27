using ComparaVentasExcel.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparaVentasExcel.Services.ComparacionExcel
{
    public class VentasComparacionService
    {
        private readonly DataAccess _dataAccess;

        public VentasComparacionService(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public (string Local, string Resultado) ConsultarVenta(
            string dbKey,
            string suc,
            string num,
            string tipo)
        {
            using var conn = _dataAccess.GetConnection(dbKey);
            conn.Open();

            string query = @"
                SELECT TOP 1 PERI_CODIGO
                FROM ventas_e
                WHERE suc_codigo = @suc AND vene_numero = @num AND cbtee_codigo = @tipo
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@suc", suc);
            cmd.Parameters.AddWithValue("@num", num);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            var result = cmd.ExecuteScalar();
            return result != null
                ? (result.ToString(), "✅ Existe")
                : ("-", "❌ No existe");
        }
    }
}

