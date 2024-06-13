using System.Diagnostics;
using GameAPI.Services;
using GameAPI.Steam;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController(GameService service)
{
    [HttpGet("{name}")]
    public async Task<List<SteamGame>> Get(string name)
    {
        var stopwatch = Stopwatch.StartNew();
        var games = await service.GetGames(name);
        stopwatch.Stop();

        Console.WriteLine("Service Time: {0} ms", stopwatch.ElapsedMilliseconds);

        return games;
    }
}
