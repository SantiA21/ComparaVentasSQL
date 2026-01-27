using ComparaVentasExcel.Data;
using System.Data;
using System.Data.SqlClient;

namespace ComparaVentasExcel.Services.Usuarios
{
    public static class UsuarioMostazaERPService
    {
        private const string DB_KEY = "MOSTAZA_ERP";

        public static DataTable ObtenerUsuarios()
        {
            string query = @"
SELECT
    Usuario As DNI,
    Usu_nomnombre As Nombre,
    usu_nomapellido as Apellido,
    Usu_nombre As NombreCinet,
    Usu_categoria As Categoria,
    Estado
FROM usuarios
ORDER BY usuario
            ";

            DataAccess daAccess = new DataAccess();

            using (SqlConnection conn = daAccess.GetConnection(DB_KEY))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
