using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace CinetCore.Services.Usuarios
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
            conn.Open();
            
            var dt = new DataTable();
            using var reader = conn.ExecuteReader(query);
            dt.Load(reader);
            
            return dt;
        }

    }
}
