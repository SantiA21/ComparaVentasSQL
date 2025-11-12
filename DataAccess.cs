using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace ComparaVentasExcel
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess()
        {
            // Obtener el ensamblado actual
            var assembly = Assembly.GetExecutingAssembly();

            // Nombre completo del recurso: <Namespace>.<NombreArchivo>
            // En este caso: "ComparaVentasExcel.dbconfig.ini"
            using (var stream = assembly.GetManifestResourceStream("ComparaVentasExcel.dbconfig.ini"))
            {
                if (stream == null)
                    throw new FileNotFoundException("No se encontró el recurso incrustado dbconfig.ini.");

                using (var reader = new StreamReader(stream))
                {
                    // Leer todo el contenido del recurso
                    string content = reader.ReadToEnd();

                    // Buscar la línea ConnectionString=
                    foreach (var line in content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
                    {
                        if (line.StartsWith("ConnectionString=", StringComparison.OrdinalIgnoreCase))
                        {
                            _connectionString = line.Substring("ConnectionString=".Length).Trim();
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("No se encontró la cadena de conexión en dbconfig.ini incrustado.");
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
