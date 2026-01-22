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
            ApplicationConfiguration.Initialize(); // recomendado para .NET 8+
            Application.Run(new FormInicio());

            if (args.Length > 0 && args[0] == "--updated")
            {
                ArrancoDesdeUpdater = true;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormInicio());
        }
    }
}
