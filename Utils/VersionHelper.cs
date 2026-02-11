using System;
using System.IO;

namespace CinetCore.Utils
{
    public static class VersionHelper
    {
        /// <summary>
        /// Lee la versión local desde el archivo version.txt ubicado junto al ejecutable.
        /// </summary>
        public static string GetLocalVersion()
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "version.txt"
            );

            if (!File.Exists(path))
            {
                // En caso de que falte el archivo, devolvemos una marca clara
                return "0.0.0 (sin version.txt)";
            }

            return File.ReadAllText(path).Trim();
        }
    }
}
