namespace GameAPI.Steam;

public class SteamGame
{
    public int AppId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool HasInfo { get; set; } = false;
}
