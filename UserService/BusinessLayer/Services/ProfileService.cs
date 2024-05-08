using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class ProfileService(IProfileRepository profileRepository, IAuthorizationService auth): IProfileService
{
    public UserData Get(string token)
    {
        UserData user = profileRepository.Get(auth.GetId(token));
        return user.Copy();
    }
}
