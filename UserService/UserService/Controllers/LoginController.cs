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
    [EnableCors("UserPolicy")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            UserData userData = service.Login(request.GetUser());

            if (string.IsNullOrEmpty(userData.Token)) return Unauthorized();

            LoginResponse loginResponse = LoginResponse.Build(userData);
            return Ok(loginResponse);
        }
        catch(Exception e)
        {
            return StatusCode(500,e.Message);
        }
    }
}