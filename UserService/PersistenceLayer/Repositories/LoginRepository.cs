using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace PersistenceLayer.Repositories;

public class LoginRepository : ILoginRepository
{
    public UserData GetUser(UserData userData)
    {
        return new();
    }
}
