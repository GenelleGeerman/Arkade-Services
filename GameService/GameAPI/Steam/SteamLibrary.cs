namespace GameAPI.Steam;

public class SteamLibrary
{
    public Dictionary<int, SteamGame> Games { get; set; } = new();
    public bool Init { get; set; } = false;
}
