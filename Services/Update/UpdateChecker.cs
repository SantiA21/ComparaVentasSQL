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
            // Usar GetStringAsync de forma síncrona con GetAwaiter().GetResult()
            // Nota: En un contexto async sería mejor usar await, pero mantenemos la firma síncrona
            string texto = httpClient.GetStringAsync(versionUrl).GetAwaiter().GetResult().Trim();
            versionRemota = new Version(texto);

            Version versionLocal =
                Assembly.GetExecutingAssembly().GetName().Version;

            return versionRemota > versionLocal;
        }
        catch (Exception)
        {
            // Sin internet, GitHub caído, formato incorrecto, etc
            return false;
        }
    }
}
