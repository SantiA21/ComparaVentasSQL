using System.Net;
using System.Reflection;

public static class UpdateChecker
{
    private static string versionUrl =
        "https://raw.githubusercontent.com/SantiA21/ComparaVentasSQL/refs/heads/main/version.txt";

    public static bool HayActualizacion(out string versionRemota)
    {
        versionRemota = "";

        using (WebClient client = new WebClient())
        {
            versionRemota = client.DownloadString(versionUrl).Trim();
        }

        Version local = Assembly.GetExecutingAssembly().GetName().Version;
        Version remota = new Version(versionRemota);

        return remota > local;
    }
}
