using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IProfileService
{
   Task<UserData> Get(string token);

   Task<UserData> Update(UserData request, string tokenString);
}
