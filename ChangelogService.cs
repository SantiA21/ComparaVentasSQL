using System.Net;

public static class ChangelogService
{
    public static string ObtenerChangelog()
    {
        string url =
            "https://raw.githubusercontent.com/SantiA21/ComparaVentasSQL/main/repo/changelog.txt";

        using (WebClient wc = new WebClient())
        {
            return wc.DownloadString(url);
        }
    }
}
