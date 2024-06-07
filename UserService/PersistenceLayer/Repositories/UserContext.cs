
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Entities;
namespace PersistenceLayer.Repositories;

public class UserContext: DbContext
{
    public UserContext(DbContextOptions<UserContext> options): base(options)
    {
        
    }
    public DbSet<UserEntity> Users { get; set; }
}
