using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IAuthorizationService
{
    string GenerateToken(UserData user);

    long GetId(string token);
}