using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Entities;

namespace PersistenceLayer.Repositories;

public class RegisterRepository : IRegisterRepository
{
    private UserContext context;
    private CrudRepository<UserEntity> crudRepo;

    public RegisterRepository(UserContext ctx)
    {
        context = ctx;
        crudRepo = new(context);
    }

    public bool Register(UserData user)
    {
        return crudRepo.Create(new(user));
    }

    public bool IsEmailExisting(UserData request)
    {
        UserEntity? user = context.Users.FirstOrDefault(u => u.Email == request.Email);
        return user != null;
    }
}
