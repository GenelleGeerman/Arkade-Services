using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserService.DTO;

[BindProperties]
public class UserRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public UserData GetUser()
    {
        return new()
        {
            Email = Email,
            Password = Password,
            FirstName = FirstName,
            LastName = LastName
        };
    }
}
