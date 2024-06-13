namespace GameAPI.Steam;

public class SteamLibrary
{
    public SteamLibrary()
    {
        Games = new();
        LastUpdated = DateTime.MinValue;
    }

    public List<SteamGame> Games { get; set; }
    public DateTime LastUpdated { get; set; }
}
