using BusinessLayer.Models;

namespace UserService.DTO;

public class ProfileRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Instagram { get; set; }
    public string Twitter { get; set; }
    public string Website { get; set; }

    public UserData GetUserData()
    {
        return new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Instagram = Instagram,
            Twitter = Twitter,
            Website = Website
        };
    }
}
