using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IRegisterService
{
    UserData Register(UserData request);

    bool IsEmailExisting(UserData request);
}
