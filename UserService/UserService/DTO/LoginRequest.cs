using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserService.DTO;

[BindProperties]
public class LoginRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    public UserData GetUser()
    {
        return new()
        {
            Email = Email, Password = Password
        };
    }
}
