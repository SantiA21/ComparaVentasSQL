namespace ComparaVentasExcel.Utils
{
    public static class VersionHelper
    {
        public static string GetLocalVersion()
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "version.txt"
            );

            return File.ReadAllText(path).Trim();
        }
    }
}
