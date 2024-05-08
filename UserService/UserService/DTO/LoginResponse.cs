using BusinessLayer.Models;

namespace UserService.DTO;

public class LoginResponse(UserData data)
{
    public string FirstName { get; set; } =data.FirstName;
    public string LastName { get; set; } = data.LastName;
    public string Email { get; set; } = data.Email;
    public string Token { get; set; } = data.Token;
}
