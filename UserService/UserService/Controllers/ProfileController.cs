using System.Linq.Expressions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Primitives;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController(IProfileService service) : ControllerBase
{
    [HttpGet]
    [EnableCors("AllowAllPolicy")]
    public IActionResult GetProfile()
    {
        try
        {
            Request.Headers.TryGetValue("Authorization", out StringValues token);
            var data = service.Get(token);
            ProfileResponse response = new(data);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
