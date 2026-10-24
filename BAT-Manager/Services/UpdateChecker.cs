using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatManager.Services
{
    public class UpdateInfo
    {
        public bool HasUpdate { get; set; }
        public string CurrentVersion { get; set; }
        public string LatestVersion { get; set; }
        public string HtmlUrl { get; set; }
        public string ReleaseTitle { get; set; }
    }

    /// <summary>
    /// Servizio responsabile del controllo degli aggiornamenti tramite GitHub Releases.
    /// </summary>
    public static class UpdateChecker
    {
        public const string RepositoryOwner = "vincenzomy03";
        public const string RepositoryName = "BAT-Manager";
        private const string ReleasesApiUrl = "https://api.github.com/repos/" + RepositoryOwner + "/" + RepositoryName + "/releases/latest";

        /// <summary>
        /// Restituisce la versione corrente dell'applicazione (es. "1.2.0").
        /// </summary>
        public static string CurrentVersionString
        {
            get
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return v != null ? string.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build) : "1.2.0";
            }
        }

        /// <summary>
        /// Confronta due versioni e determina se la versione trovata su GitHub è più recente di quella corrente.
        /// </summary>
        public static bool IsNewerVersion(string latestTag, string currentVersion)
        {
            if (string.IsNullOrWhiteSpace(latestTag) || string.IsNullOrWhiteSpace(currentVersion))
                return false;

            string cleanLatest = latestTag.Trim().TrimStart('v', 'V');
            string cleanCurrent = currentVersion.Trim().TrimStart('v', 'V');

            Version vLatest, vCurrent;
            if (Version.TryParse(cleanLatest, out vLatest) && Version.TryParse(cleanCurrent, out vCurrent))
            {
                return vLatest > vCurrent;
            }

            return false;
        }

        /// <summary>
        /// Controlla in modo asincrono se esiste una nuova release disponibile su GitHub.
        /// </summary>
        public static async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            var info = new UpdateInfo
            {
                HasUpdate = false,
                CurrentVersion = CurrentVersionString,
                LatestVersion = CurrentVersionString,
                HtmlUrl = "https://github.com/" + RepositoryOwner + "/" + RepositoryName + "/releases/latest",
                ReleaseTitle = string.Empty
            };

            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BAT-Manager-UpdateChecker", info.CurrentVersion));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

                    string json = await client.GetStringAsync(ReleasesApiUrl).ConfigureAwait(false);

                    var tagMatch = Regex.Match(json, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"");
                    var urlMatch = Regex.Match(json, "\"html_url\"\\s*:\\s*\"([^\"]+)\"");
                    var nameMatch = Regex.Match(json, "\"name\"\\s*:\\s*\"([^\"]+)\"");

                    if (tagMatch.Success)
                    {
                        string tag = tagMatch.Groups[1].Value;
                        info.LatestVersion = tag.TrimStart('v', 'V');

                        if (urlMatch.Success)
                        {
                            info.HtmlUrl = urlMatch.Groups[1].Value;
                        }

                        if (nameMatch.Success)
                        {
                            info.ReleaseTitle = nameMatch.Groups[1].Value;
                        }

                        info.HasUpdate = IsNewerVersion(tag, info.CurrentVersion);
                    }
                }
            }
            catch
            {
                // In caso di mancata connessione o errore di rete, info.HasUpdate rimane false
            }

            return info;
        }
    }
}
