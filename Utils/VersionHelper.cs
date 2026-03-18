using System;
using System.IO;

namespace CinetCore.Utils
{
    public static class VersionHelper
    {
        /// <summary>

        /// </summary>
        public static string GetLocalVersion()
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "version.txt"
            );

            if (!File.Exists(path))
            {

                return "0.0.0 (sin version.txt)";
            }

            return File.ReadAllText(path).Trim();
        }
    }
}
