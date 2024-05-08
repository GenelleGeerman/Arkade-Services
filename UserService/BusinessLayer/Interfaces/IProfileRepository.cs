using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IProfileRepository
{
    UserData Get(long userId);
}
