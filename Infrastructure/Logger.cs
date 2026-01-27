using System;
using System.IO;

namespace ComparaVentasExcel.Infrastructure
{
    public static class Logger
    {
        private static readonly string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static Logger()
        {
            // Creo la carpeta Logs si no existe
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

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

            }
        }


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
               
            }
        }
    }
}
