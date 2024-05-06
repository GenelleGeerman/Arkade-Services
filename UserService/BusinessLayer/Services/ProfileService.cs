using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class ProfileService(IProfileRepository profileRepository)
{
    public UserData Get(int userId)
    {
        UserData user = profileRepository.Get(userId);
        user.Password = string.Empty;
        user.Salt = Array.Empty<byte>();
        return user;
    }
}
