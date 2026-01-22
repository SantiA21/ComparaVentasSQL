using System;
using System.Net;
using System.Reflection;

public static class UpdateChecker
{
    private static string versionUrl =
        "https://raw.githubusercontent.com/SantiA21/ComparaVentasSQL/refs/heads/main/version.txt";

    public static bool HayActualizacion(out Version versionRemota)
    {
        versionRemota = null;

        try
        {
            using (WebClient wc = new WebClient())
            {
                string texto = wc.DownloadString(versionUrl).Trim();
                versionRemota = new Version(texto);
            }

            Version versionLocal =
                Assembly.GetExecutingAssembly().GetName().Version;

            return versionRemota > versionLocal;
        }
        catch
        {
            // Sin internet, GitHub caído, etc
            return false;
        }
    }
}
