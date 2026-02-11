using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace CinetCore.Utils
{
    public static class UserMessageHelper
    {
        /// <summary>
        /// Devuelve un mensaje amigable para el usuario según el tipo de error.
        /// </summary>
        /// <param name="contexto">Descripción breve de qué se estaba haciendo, por ej. "al consultar la venta".</param>
        /// <param name="ex">Excepción capturada.</param>
        public static string GetFriendlyMessage(string contexto, Exception ex)
        {
            // Timeouts de SQL Server
            if (ex is SqlException sqlEx && sqlEx.Number == -2)
            {
                return $"La operación {contexto} superó el tiempo de espera.\n\n" +
                       "Verificá la conexión con el servidor SQL y volvé a intentarlo.";
            }

            // Timeouts / cancelaciones en HTTP
            if (ex is TaskCanceledException)
            {
                return $"La operación {contexto} tardó demasiado en responder.\n\n" +
                       "Revisá tu conexión a internet e intentá nuevamente.";
            }

            if (ex is HttpRequestException)
            {
                return $"No se pudo completar la operación {contexto} por un problema de conexión " +
                       "a internet o al servicio remoto.\n\nDetalle técnico: " + ex.Message;
            }

            // Genérico
            return $"Se produjo un error {contexto}.\n\nDetalle técnico: {ex.Message}";
        }
    }
}

