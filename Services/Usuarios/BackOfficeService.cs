using ComparaVentasExcel.Data;
using ComparaVentasExcel.Infrastructure;
using ComparaVentasExcel.Models;
using System.Data;
using System.Data.SqlClient;

namespace ComparaVentasExcel.Services.Usuarios
{
    public class BackOfficeService
    {
        private readonly DataAccess _dataAccess;

        public BackOfficeService(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public void ProbarConexion(ConexionBackOffice config)
        {
            using var conn = _dataAccess.GetRemoteConnection(config);
            conn.Open();
        }

        public DataTable EjecutarConsultaUsuarios(ConexionBackOffice config)
        {
            const string query = "SELECT Usuario As DNI,   Usu_nomnombre As Nombre,    usu_nomapellido as Apellido,    Usu_nombre As NombreCinet,   Usu_categoria As Categoria,    Estado FROM usuarios ORDER BY usuario";

            Logger.LogQuery(query);

            using var conn = _dataAccess.GetRemoteConnection(config);
            using var cmd = new SqlCommand(query, conn);
            using var da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            return dt;
        }

    }
}
