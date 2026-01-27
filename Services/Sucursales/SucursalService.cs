using ComparaVentasExcel.Data;
using ComparaVentasExcel.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ComparaVentasExcel.Services.Sucursales
{
    public class SucursalService
    {
        private readonly DataAccess dataAccess;

        public SucursalService(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public DataTable ObtenerSucursales(string dbKey)
        {
            DataTable dt = new DataTable();
            string query = GetQueryByDatabase(dbKey);

            using (SqlConnection conexion = dataAccess.GetConnection(dbKey))
            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                Logger.LogQuery(cmd.CommandText);
                conexion.Open();
                adapter.Fill(dt);
            }

            return dt;
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
