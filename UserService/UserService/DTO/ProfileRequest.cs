using BusinessLayer.Models;

namespace UserService.DTO;

public class ProfileRequest
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Instagram { get; set; } = string.Empty;
    public string Twitter { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;

    public UserData GetUserData()
    {
        return new()
        {
            Username = Username,
            FirstName = FirstName,
            LastName = LastName,
            Instagram = Instagram,
            Twitter = Twitter,
            Website = Website
        };
    }
}
