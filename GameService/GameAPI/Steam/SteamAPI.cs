using Newtonsoft.Json.Linq;

namespace GameAPI.Steam
{
    public class SteamAPI
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly SteamLibrary steamLibrary = new SteamLibrary();

        public async Task<List<SteamGame>> GetGames(string name)
        {
            var games = steamLibrary.Games
                .Where(g => g.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            TypoCheck(g.Name, name) <= 2)
                .ToList();

            if (!games.Any())
            {
                var newGames = await FetchGamesFromAPI(name);
                steamLibrary.Games.AddRange(newGames);
                steamLibrary.LastUpdated = DateTime.Now;

                games = newGames
                    .Where(g => g.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                TypoCheck(g.Name, name) <= 2)
                    .ToList();
            }

            return games;
        }

        private async Task<List<SteamGame>> FetchGamesFromAPI(string name)
        {
            string url = $"https://api.steampowered.com/ISteamApps/GetAppList/v2/";
            var response = await httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);
            var apps = json["applist"]["apps"].ToObject<List<JObject>>();

            var matchingApps = apps
                .Where(a => a["name"].ToString().IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            TypoCheck(a["name"].ToString(), name) <= 2)
                .ToList();

            var newGames = new List<SteamGame>();

            foreach (var app in matchingApps)
            {
                int appId = app["appid"].Value<int>();
                string appDetailsUrl = $"https://store.steampowered.com/api/appdetails?appids={appId}";
                var appDetailsResponse = await httpClient.GetStringAsync(appDetailsUrl);
                var appDetailsJson = JObject.Parse(appDetailsResponse);

                if (appDetailsJson[appId.ToString()]["success"].Value<bool>())
                {
                    var data = appDetailsJson[appId.ToString()]["data"];
                    var steamGame = new SteamGame(data);
                    newGames.Add(steamGame);
                }
            }

            return newGames;
        }

        private int TypoCheck(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) { return m; }
            if (m == 0) { return n; }

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}
