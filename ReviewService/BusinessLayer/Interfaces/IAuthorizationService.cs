using BusinessLayer.Models;
using Microsoft.Extensions.Primitives;

namespace BusinessLayer.Interfaces;

public interface IAuthorizationService
{
    int GetUserId(string token);
}