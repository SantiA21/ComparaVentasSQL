using System;
using System.Data.SqlClient;
using System.IO;

namespace ComparadorExcelSQL
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess()
        {
            // Ruta al archivo de configuración en la misma carpeta del ejecutable
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconfig.ini");

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("No se encontró el archivo de configuración: " + configPath);
            }

            // Leer el archivo línea por línea y buscar "ConnectionString="
            foreach (var line in File.ReadAllLines(configPath))
            {
                if (line.StartsWith("ConnectionString=", StringComparison.OrdinalIgnoreCase))
                {
                    _connectionString = line.Substring("ConnectionString=".Length).Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("No se encontró la cadena de conexión en dbconfig.ini");
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
