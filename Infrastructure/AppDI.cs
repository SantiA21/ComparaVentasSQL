using Microsoft.Extensions.DependencyInjection;
using System;

namespace CinetCore.Infrastructure
{
    public static class AppDI
    {
        private static IServiceProvider _serviceProvider;

        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("Inyección de Dependencias no configurada.");
            
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
