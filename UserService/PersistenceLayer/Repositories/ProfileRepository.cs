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
        UserEntity entity = new(request);
        crudRepo.Update(entity);
        await context.SaveChangesAsync();
        return entity.GetUserData();
    }

    public async Task<bool> Delete(long id)
    {
        return crudRepo.Delete(crudRepo.GetById(id));
    }
}
