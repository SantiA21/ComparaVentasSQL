using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

public static class UpdateChecker
{
    private static readonly string versionUrl =
        "https://raw.githubusercontent.com/SantiA21/ComparaVentasSQL/refs/heads/main/version.txt";
    
    private static readonly string githubToken = "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";

    private static readonly HttpClient httpClient = CreateHttpClient();

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"token {githubToken}");
        client.DefaultRequestHeaders.Add("User-Agent", "CinetCoreApp");
        return client;
    }

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
