using GameAPI.Steam;

namespace GameAPI.Services;

public class GameService(SteamAPI steam)
{
    public async Task<List<SteamGame>> GetGames(string name)
    {
        return await steam.GetGames(name);
    }
}
