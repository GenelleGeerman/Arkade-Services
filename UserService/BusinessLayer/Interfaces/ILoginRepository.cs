using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface ILoginRepository
{
    UserData GetUser(UserData userData);
}
