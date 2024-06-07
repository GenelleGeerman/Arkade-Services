using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class ProfileService(IProfileRepository profileRepository, IAuthorizationService auth): IProfileService
{
    public async Task<UserData> Get(string token)
    {
        UserData user = await  profileRepository.Get(auth.GetId(token));
        
        return user.Copy();
    }

    public async Task<UserData> Update(UserData request, string token)
    {
        long id = auth.GetId(token);
        string email = auth.GetEmail(token);
        request.Id = id;
        request.Email = email;
        UserData response = await profileRepository.Update(request);
        return response;
    }

    public async Task<bool> Delete(string token)
    {
        return await profileRepository.Delete(auth.GetId(token));
    }
}
