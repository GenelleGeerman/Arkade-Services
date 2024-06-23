using BusinessLayer.Models;

namespace UserService.DTO;

public class ProfileResponse(UserData data)
{
    public string Username { get; set; } = data.Username;
    public string FirstName { get; set; } = data.FirstName;
    public string LastName { get; set; } = data.LastName;
    public string Email { get; set; } = data.Email;
    public string Instagram { get; set; } = data.Instagram;
    public string Twitter { get; set; } = data.Twitter;
    public string Website { get; set; } = data.Website;
}
