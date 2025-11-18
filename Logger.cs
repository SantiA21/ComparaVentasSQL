using System;
using System.IO;

namespace ComparaVentasExcel
{
    public static class Logger
    {
        private static readonly string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static Logger()
        {
            // Crea la carpeta Logs si no existe
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        // ------------------------------------------
        // LOG DE ERRORES
        // ------------------------------------------
        public static void LogError(Exception ex)
        {
            try
            {
                string file = Path.Combine(logDir, $"error_{DateTime.Now:yyyy-MM-dd}.txt");
                string msg = $"--------------------\n[{DateTime.Now:HH:mm:ss}] ERROR: {ex.Message}\nSTACKTRACE: {ex.StackTrace}\n";

                File.AppendAllText(file, msg);
            }
            catch
            {
                // Si falla el log, no hacer nada para no romper la app
            }
        }

        // ------------------------------------------
        // LOG DE QUERYS
        // ------------------------------------------
        public static void LogQuery(string query)
        {
            try
            {
                string file = Path.Combine(logDir, $"query_{DateTime.Now:yyyy-MM-dd}.txt");
                string msg = $"--------------------\n[{DateTime.Now:HH:mm:ss}] QUERY REALIZADA: {query}\n";

                File.AppendAllText(file, msg);
            }
            catch
            {
                // No romper si falla el log
            }
        }
    }
}
