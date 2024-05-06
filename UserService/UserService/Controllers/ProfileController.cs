using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController
{
    [HttpPost]
    [EnableCors("UserPolicy")]
    public string UserInfo()
    {
        return "lol";
    }
}
