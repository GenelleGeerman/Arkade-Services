using Newtonsoft.Json.Linq;

namespace GameAPI.Steam;

public class SteamGame
{
    public SteamGame(JToken data)
    {
        AppId = data["steam_appid"].Value<int>();
        Name = data["name"].Value<string>();
        Developer = data["developers"] != null ? string.Join(", ", data["developers"].ToObject<List<string>>()) : "Unknown";
        Publisher = data["publishers"] != null ? string.Join(", ", data["publishers"].ToObject<List<string>>()) : "Unknown";
        Genre = data["genres"] != null ? string.Join(", ", data["genres"].Select(g => g["description"].Value<string>())) : "Unknown";
        ReleaseDate = data["release_date"]?["date"]?.Value<string>() ?? "Unknown";
        Description = data["short_description"]?.Value<string>() ?? "No description available";
        Price = data["price_overview"]?["final"].Value<decimal>() / 100 ?? 0; // Price is in cents
    }

    public int AppId { get; set; }
    public string Name { get; set; }
    public string Developer { get; set; }
    public string Publisher { get; set; }
    public string Genre { get; set; }
    public string ReleaseDate { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}