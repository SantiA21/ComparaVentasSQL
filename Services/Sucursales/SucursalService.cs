using CinetCore.Data;
using CinetCore.Infrastructure;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CinetCore.Services.Sucursales
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

                Logger.LogQuery(sql);
                conn.Open();
                cmd.ExecuteNonQuery();
                Logger.LogInfo($"[SUCURSALES] Sucursal FE '{pdv}' insertada correctamente en {dbKey}.");
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

        public static DataTable ObtenerSucursalesRemotas(string connectionString)
        {
            string query = @"
use backoffice
declare @infoCaja table([caja] varchar(20), [equipo] varchar(259), [version] varchar(200) );

insert into @infoCaja
select distinct caja,EQUIPO,valor from (
select RANK() OVER (
    PARTITION BY caja, parametro
    ORDER BY fechatrans desc) rango, *
from hparamloc
where parametro = 'VERSION') subQuery
where rango = 1
order by equipo

SELECT vene_caja As NumCaja, suc_codigo As Sucursal, equipo As Hostname
FROM (
    SELECT 
        ROW_NUMBER() OVER (PARTITION BY v.vene_caja ORDER BY v.vene_fecha DESC) AS rn,
        v.vene_caja,
        v.suc_codigo,
        i.equipo
    FROM VENTAS_E v
    INNER JOIN @infoCaja i ON v.vene_caja = i.caja COLLATE SQL_Latin1_General_CP1_CI_AS
    WHERE v.vene_caja != ''
) AS subquery
WHERE rn = 1
ORDER BY equipo;
";
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(CinetCore.Data.DataAccess.EnsureTrustServerCertificate(connectionString)))
            using (var cmd = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                Logger.LogQuery(query);
                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }
    }
}
