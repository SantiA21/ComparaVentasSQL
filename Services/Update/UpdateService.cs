using System.Net.Http;
using System.Text.Json;

namespace CinetCore.Services.Update
{
    public class UpdateService
    {
        private const string VersionUrl =
            "URL_RAW_DE_TU_version.json";

        public async Task<RemoteVersion?> GetRemoteVersionAsync()
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(VersionUrl);
            return JsonSerializer.Deserialize<RemoteVersion>(json);
        }

        public bool IsNewer(string remote, string local)
        {
            return new Version(remote) > new Version(local);
        }
    }
}
