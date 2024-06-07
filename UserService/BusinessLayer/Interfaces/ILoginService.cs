using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface ILoginService
{
    Task<UserData> Login(UserData request);
}
