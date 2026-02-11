using System;
using System.IO;

namespace CinetCore.Infrastructure
{
    public static class Logger
    {
        private static readonly string logDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static Logger()
        {
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        public static void LogError(Exception ex)
        {
            try
            {
                string file = Path.Combine(logDir, $"error_{DateTime.Now:yyyy-MM-dd}.txt");
                string msg =
$@"--------------------
[{DateTime.Now:HH:mm:ss}] ERROR: {ex.Message}
STACKTRACE:
{ex.StackTrace}
";

                File.AppendAllText(file, msg);
            }
            catch (Exception logEx)
            {
                // Si falla el logging, intentar escribir a un archivo de emergencia
                try
                {
                    string emergencyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logger_error.txt");
                    File.AppendAllText(emergencyFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error al escribir log: {logEx.Message}\n");
                }
                catch { /* Si incluso esto falla, ignorar silenciosamente */ }
            }
        }

        public static void LogQuery(string query)
        {
            try
            {
                string file = Path.Combine(logDir, $"query_{DateTime.Now:yyyy-MM-dd}.txt");
                string msg =
$@"--------------------
[{DateTime.Now:HH:mm:ss}] QUERY REALIZADA:
{query}
";

                File.AppendAllText(file, msg);
            }
            catch (Exception logEx)
            {
                // Si falla el logging, intentar escribir a un archivo de emergencia
                try
                {
                    string emergencyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logger_error.txt");
                    File.AppendAllText(emergencyFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error al escribir log de query: {logEx.Message}\n");
                }
                catch { /* Si incluso esto falla, ignorar silenciosamente */ }
            }
        }

        // 👉 NUEVO
        public static void LogInfo(string message)
        {
            try
            {
                string file = Path.Combine(logDir, $"info_{DateTime.Now:yyyy-MM-dd}.txt");
                string msg =
$@"--------------------
[{DateTime.Now:HH:mm:ss}] INFO: {message}
";

                File.AppendAllText(file, msg);
            }
            catch (Exception logEx)
            {
                // Si falla el logging, intentar escribir a un archivo de emergencia
                try
                {
                    string emergencyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logger_error.txt");
                    File.AppendAllText(emergencyFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error al escribir log de info: {logEx.Message}\n");
                }
                catch { /* Si incluso esto falla, ignorar silenciosamente */ }
            }
        }
    }
}
