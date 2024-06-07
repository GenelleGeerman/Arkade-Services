using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface ILoginRepository
{
    Task<UserData> GetUser(UserData userData);
}
