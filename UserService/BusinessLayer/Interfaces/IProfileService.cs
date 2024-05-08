using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IProfileService
{
   UserData Get(string token);
}
