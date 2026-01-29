using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparaVentasExcel

{
    static class Program
    {
        public static bool ArrancoDesdeUpdater = false;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Verificar argumentos antes de inicializar la aplicación
            if (args.Length > 0 && args[0] == "--updated")
            {
                ArrancoDesdeUpdater = true;
            }

            // Configuración de la aplicación (recomendado para .NET 8+)
            ApplicationConfiguration.Initialize();
            
            // Ejecutar la aplicación principal
            Application.Run(new FormInicio());
        }
    }
}
