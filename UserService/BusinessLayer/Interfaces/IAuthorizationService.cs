using BusinessLayer.Models;
using Microsoft.Extensions.Primitives;

namespace BusinessLayer.Interfaces;

public interface IAuthorizationService
{
    string GenerateToken(UserData user);

    long GetId(string token);

    string GetEmail(string token);
}