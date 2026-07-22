using System.Reflection;

namespace CinetCore.Utils
{
    public static class AppInfo
    {
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
    }
}
