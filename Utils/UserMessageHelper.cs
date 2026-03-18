using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace CinetCore.Utils
{
    public static class UserMessageHelper
    {

        public static string GetFriendlyMessage(string contexto, Exception ex)
        {

            if (ex is SqlException sqlEx && sqlEx.Number == -2)
            {
                return $"La operación {contexto} superó el tiempo de espera.\n\n" +
                       "Verificá la conexión con el servidor SQL y volvé a intentarlo.";
            }


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

