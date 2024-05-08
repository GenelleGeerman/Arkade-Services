using BusinessLayer.Models;

namespace UserService.DTO;

public class LoginResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    
    public static LoginResponse Build(UserData userData)
    {
        return new()
        {
            FirstName = userData.FirstName,
            LastName = userData.LastName,
            Email = userData.Email,
            Token = userData.Token
        };
    }
}