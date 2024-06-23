using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;

namespace RepositoryLayer;

public class ReviewContext : DbContext
{
    public ReviewContext(DbContextOptions<ReviewContext> options) : base(options) { }

    public DbSet<ReviewEntity> Reviews { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>()
            .Property(e => e.RowVersion)
            .IsRowVersion();
    }
}
