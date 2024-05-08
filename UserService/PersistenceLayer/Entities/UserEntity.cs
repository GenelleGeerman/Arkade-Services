using BusinessLayer.Models;

namespace PersistenceLayer.Entities;

public class UserEntity
{
    public long Id { get; set; } = -1;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public byte[] ProfilePicture {get; set;} = Array.Empty<byte>();
    public byte[] Salt { get; set; } = Array.Empty<byte>();

    public UserEntity() { }

    public UserEntity(UserData data)
    {
        Id = data.Id;
        FirstName = data.FirstName;
        LastName = data.LastName;
        Email = data.Email;
        Password = data.Password;
        ProfilePicture = data.ProfilePicture;
        Salt = data.Salt;
    }

    public UserData GetUserData()
    {
        return new()
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            ProfilePicture = ProfilePicture,
            Salt = Salt
        };
    }
}
