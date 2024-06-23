using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using PersistenceLayer.Entities;

namespace PersistenceLayer.Repositories;

public class ProfileRepository : IProfileRepository
{
    private UserContext context;
    private CrudRepository<UserEntity> crudRepo;

    public ProfileRepository(UserContext ctx)
    {
        context = ctx;
        crudRepo = new(context);
    }

    public async Task<UserData> Get(long userId)
    {
        UserEntity entity = crudRepo.GetById(userId);
        return entity == null ? new() : entity.GetUserData();
    }

    public async Task<UserData> Update(UserData request)
    {
        UserEntity entity = new(request); // Assuming UserEntity maps to UserData
        context.Update(entity); // Mark entity as modified
        await context.SaveChangesAsync();
        await context.Entry(entity).ReloadAsync();
        return entity.GetUserData();
    }


    public async Task<bool> Delete(long id)
    {
        return crudRepo.Delete(crudRepo.GetById(id));
    }
}
