using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using UserService.DTO;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController(IProfileService service) : ControllerBase
{
    [HttpGet]
    [EnableCors("AllowAllPolicy")]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
                return Unauthorized("Authorization header missing");
            Console.WriteLine(token);
            string? tokenString = token.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(tokenString)) return Unauthorized("Invalid token or token is empty.");

            UserData data = await service.Get(tokenString);
            ProfileResponse response = new(data);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{id}")]
    [EnableCors("AllowAllPolicy")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            UserData data = await service.GetProfile(id);
            ProfileResponse response = new(data);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    [EnableCors("AllowAllPolicy")]
    public async Task<IActionResult> Update([FromBody] ProfileRequest request)
    {
        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
                return Unauthorized("Authorization header missing");

            string? tokenString = token.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(tokenString)) return Unauthorized("Invalid token or token is empty.");

            UserData data = await service.Update(request.GetUserData(), tokenString);
            ProfileResponse response = new(data);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        try
        { 
            if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
                return Unauthorized("Authorization header missing");

            string? tokenString = token.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(tokenString)) return Unauthorized("Invalid token or token is empty.");
            bool isDeleted = await service.Delete(tokenString);

            if (!isDeleted) return NotFound();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
