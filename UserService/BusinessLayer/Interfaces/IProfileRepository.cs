using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IProfileRepository
{
    Task<UserData> Get(long userId);

    Task<UserData> Update(UserData request);

    Task<bool> Delete(long getId);
}
