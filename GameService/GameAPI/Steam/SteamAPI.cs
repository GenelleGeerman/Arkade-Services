using Newtonsoft.Json.Linq;

namespace GameAPI.Steam;

public class SteamApi
{
    private readonly HttpClient httpClient = new();
    private readonly SteamLibrary steamLibrary = new();

    private void InitializeLibrary()
    {
        if (steamLibrary.Init) return;
        steamLibrary.Init = true;

        string url = "https://api.steampowered.com/ISteamApps/GetAppList/v2/";
        var response = httpClient.GetStringAsync(url).Result;
        var json = JObject.Parse(response);
        JObject[] apps = json["applist"]["apps"].ToObject<JObject[]>();

        if (apps == null) return;

        foreach (JObject t in apps)
        {
            SteamGame game = new() { AppId = t["appid"].Value<int>(), Name = t["name"].Value<string>() };
            Console.WriteLine($"Created: {game.AppId} - {game.Name}");
            steamLibrary.Games.TryAdd(game.AppId, game);
        }
    }

    public SteamGame[] GetGames(string name)
    {
        InitializeLibrary();
        return steamLibrary.Games.Values
            .Where(g => g.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 || TypoCheck(g.Name, name) <= 2)
            .ToArray();
    }

    public SteamGame[] GetAllGames()
    {
        InitializeLibrary();
        return steamLibrary.Games.Values.ToArray();
    }

    public async Task<SteamGame> GetById(int id)
    {
        InitializeLibrary();
        bool result = steamLibrary.Games.TryGetValue(id, out SteamGame? game);

        if (result && game is { HasInfo: true }) return game;

        string appDetailsUrl = $"https://store.steampowered.com/api/appdetails?appids={id}";
        var appDetailsResponse = await httpClient.GetStringAsync(appDetailsUrl);
        var appDetailsJson = JObject.Parse(appDetailsResponse);

        if (!(appDetailsJson[id.ToString()]?["success"] ?? "false").Value<bool>()) return new();
        var data = appDetailsJson[id.ToString()]?["data"];
        if (data == null) return new();
        game = CreateGame(data);
        steamLibrary.Games.TryAdd(game.AppId, game);
        return game;
    }

    private int TypoCheck(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return m;

        if (m == 0) return n;

        for (int i = 0; i <= n; d[i, 0] = i++) { }

        for (int j = 0; j <= m; d[0, j] = j++) { }

        for (int i = 1; i <= n; i++)
        for (int j = 1; j <= m; j++)
        {
            int cost = t[j - 1] == s[i - 1] ? 0 : 1;
            d[i, j] = Math.Min(
                Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                d[i - 1, j - 1] + cost);
        }

        return d[n, m];
    }

    private SteamGame CreateGame(JToken data)
    {
        return new()
        {
            AppId = (data["steam_appid"] ?? data["appid"]).Value<int>(),

            Name = (data["name"] ?? "unknown").Value<string>() ?? "unknown",
            Developer = data["developers"] != null
                ? string.Join(", ", data["developers"].ToObject<List<string>>())
                : "Unknown",
            Publisher = data["publishers"] != null
                ? string.Join(", ", data["publishers"].ToObject<List<string>>())
                : "Unknown",
            Genre = data["genres"] != null
                ? string.Join(", ", data["genres"].Select(g => g["description"].Value<string>()))
                : "Unknown",
            ReleaseDate = data["release_date"]?["date"]?.Value<string>() ?? "Unknown",
            Description = data["short_description"]?.Value<string>() ?? "No description available",
            Price = data["price_overview"]?["final"].Value<decimal>() / 100 ?? 0 // Price is in cents
        };
    }
}
