using CinetCore.Infrastructure;
using CinetCore;
using Microsoft.VisualBasic;

namespace CinetCore.Utils
{
    public static class SeguridadHelper
    {
        private const string CLAVE = "Cinet2026@";

        public static bool ValidarClave()
        {
            using (var frm = new FrmClave())
            {
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    Logger.LogInfo("Validación de clave cancelada por el usuario");
                    return false;
                }

                if (frm.ClaveIngresada != CLAVE)
                {
                    Logger.LogInfo("Clave incorrecta ingresada");
                    return false;
                }

                return true;
            }
        }
    }
}
