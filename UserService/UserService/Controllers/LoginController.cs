using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController(ILoginService service) : ControllerBase
{
    [HttpPost]
    [EnableCors("AllowAllPolicy")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            UserData userData = service.Login(request.GetUser());

            if (string.IsNullOrEmpty(userData.Token)) return Unauthorized();

            return Ok(new LoginResponse(userData));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
