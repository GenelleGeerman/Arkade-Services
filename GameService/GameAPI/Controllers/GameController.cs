using GameAPI.Steam;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController(SteamApi service) : ControllerBase
{
    [HttpGet("{name}")]
    public IActionResult Get(string name)
    {
        try
        {
            return Ok(service.GetGames(name));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            return Ok(await service.GetById(id));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(service.GetAllGames());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
