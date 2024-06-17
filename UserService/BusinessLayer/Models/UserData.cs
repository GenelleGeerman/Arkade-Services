namespace BusinessLayer.Models;

public class UserData
{
    public long Id { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public string Token { get; set; } = string.Empty;
    public string Instagram { get; set; }= string.Empty;
    public string Twitter { get; set; }= string.Empty;
    public string Website { get; set; }= string.Empty;

    public UserData Copy()
    {
        UserData copy = (UserData)MemberwiseClone();
        copy.Password = string.Empty;
        copy.Salt = Array.Empty<byte>();
        return copy;
    }

    public UserData SafeCopy()
    {
        UserData data = Copy();
        data.Email = string.Empty;
        return data;
    }
}
