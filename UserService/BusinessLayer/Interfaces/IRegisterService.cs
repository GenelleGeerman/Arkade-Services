using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IRegisterService
{
    Task<UserData> Register(UserData request);

    bool IsEmailExisting(UserData request);
}
