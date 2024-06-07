using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using PersistenceLayer.Entities;

namespace PersistenceLayer.Repositories;

public class LoginRepository : ILoginRepository
{
    private UserContext context;
    private CrudRepository<UserEntity> crudRepo;

    public LoginRepository(UserContext ctx)
    {
        context = ctx;
        crudRepo = new(context);
    }

    public async Task<UserData> GetUser(UserData userData)
    {
        UserEntity entity = await GetByEmail(userData.Email);
        return entity.GetUserData();
    }

    public async Task<UserEntity> GetByEmail(string email)
    {
        return context.Users.FirstOrDefault(u => u.Email == email) ?? new();
    }
}
