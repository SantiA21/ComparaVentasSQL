using CinetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinetCore

{
    static class Program
    {
        public static bool ArrancoDesdeUpdater = false;

        [STAThread]
        static void Main(string[] args)
        {

            if (args.Length > 0 && args[0] == "--updated")
            {
                ArrancoDesdeUpdater = true;
            }


            ApplicationConfiguration.Initialize();
            

            Application.Run(new FormInicio());
        }
    }
}
