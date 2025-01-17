using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController(IRegisterService service) : ControllerBase
{
    [HttpPost]
    [EnableCors("AllowAllPolicy")]
    public async Task<IActionResult> Register([FromBody] UserRequest request)
    {
        try
        {
            if (service.IsEmailExisting(request.GetUser())) return Unauthorized("Email Exists");
            UserData user = await service.Register(request.GetUser());
            Console.WriteLine(user.Id);
            return string.IsNullOrEmpty(user.Token) ? BadRequest() : Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
