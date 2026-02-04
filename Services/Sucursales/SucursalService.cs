using ComparaVentasExcel.Data;
using ComparaVentasExcel.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ComparaVentasExcel.Services.Sucursales
{
    public class SucursalService
    {
        private readonly DataAccess _dataAccess;

        public SucursalService(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public DataTable ObtenerSucursales(string dbKey)
        {
            DataTable dt = new DataTable();
            string query = GetQueryByDatabase(dbKey);

            using (SqlConnection conexion = _dataAccess.GetConnection(dbKey))
            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                Logger.LogQuery(cmd.CommandText);
                conexion.Open();
                adapter.Fill(dt);
            }

            return dt;
        }

        public void InsertarSucursal(string dbKey, string pdv)
        {
            string sql = @"
                INSERT INTO SUCURSALES
                VALUES (
                    @pdv,
                    @descripcion,
                    NULL, NULL, NULL, NULL, NULL,
                    'FE',
                    'FE',
                    NULL, NULL
                )";

            using (SqlConnection conn = _dataAccess.GetConnection(dbKey))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@pdv", SqlDbType.VarChar).Value = pdv;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar)
                               .Value = $"{pdv}.DL.";

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private string GetQueryByDatabase(string dbKey)
        {
            if (dbKey.Equals("MOSTAZA_ERP", StringComparison.OrdinalIgnoreCase))
            {
                return @"
SELECT *
FROM (
    SELECT DISTINCT 
        ve.vene_caja AS Caja,
        su.SUC_CODIGO AS Sucursal, 
        ve.PERI_CODIGO AS Local 
    FROM SUCURSALES su
    INNER JOIN VENTAS_E ve ON ve.SUC_CODIGO = su.SUC_CODIGO
    WHERE su.SUC_CODIGO >= '1500'
      AND su.SUC_LOCAL = 'FE'
      AND ve.VENE_FECHA > GETDATE() - 30
) AS X
ORDER BY X.Local, TRY_CAST(X.Caja AS INT);";
            }

            if (dbKey.Equals("GMG_ERP", StringComparison.OrdinalIgnoreCase))
            {
                return @"
SELECT *
FROM (
    SELECT DISTINCT 
        ve.vene_caja AS Caja,
        su.SUC_CODIGO AS Sucursal, 
        ve.PERI_CODIGO AS Local 
    FROM SUCURSALES su
    INNER JOIN VENTAS_E ve ON ve.SUC_CODIGO = su.SUC_CODIGO
    WHERE su.SUC_CODIGO >= '0300'
      AND su.SUC_LOCAL = 'FE'
      AND ve.VENE_FECHA > GETDATE() - 30
) AS X
ORDER BY X.Local, TRY_CAST(X.Caja AS INT);";
            }

            throw new ArgumentException("Base de datos no soportada");
        }
    }
}
