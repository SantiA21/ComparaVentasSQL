using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

public static class UpdateChecker
{
    private static readonly string versionUrl =
        "https://raw.githubusercontent.com/SantiA21/ComparaVentasSQL/refs/heads/main/version.txt";
    
    private static readonly HttpClient httpClient = new HttpClient();

    public static bool HayActualizacion(out Version versionRemota)
    {
        versionRemota = null;

        try
        {

            string texto = httpClient.GetStringAsync(versionUrl).GetAwaiter().GetResult().Trim();
            versionRemota = new Version(texto);

            Version versionLocal =
                Assembly.GetExecutingAssembly().GetName().Version;

            return versionRemota > versionLocal;
        }
        catch (Exception)
        {

            return false;
        }
    }
}
