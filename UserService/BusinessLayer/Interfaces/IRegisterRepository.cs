using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IRegisterRepository
{
    bool Register(UserData user);

    bool IsEmailExisting(UserData request);
}
