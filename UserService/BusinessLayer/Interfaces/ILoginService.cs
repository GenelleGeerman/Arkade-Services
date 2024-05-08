using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface ILoginService
{
    UserData Login(UserData request);
}
