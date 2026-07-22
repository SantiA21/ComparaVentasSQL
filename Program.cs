using CinetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

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

            // Configure Dependency Injection
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddSingleton<CinetCore.Data.DataAccess>();
            
            var provider = services.BuildServiceProvider();
            CinetCore.Infrastructure.AppDI.Configure(provider);

            Application.Run(new FormInicio());
        }
    }
}
