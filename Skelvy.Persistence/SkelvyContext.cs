using Microsoft.EntityFrameworkCore;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence
{
  public class SkelvyContext : DbContext
  {
    public SkelvyContext(DbContextOptions options)
      : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserProfilePhoto> UserProfilePhotos { get; set; }
    public DbSet<Drink> Drinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkelvyContext).Assembly);
    }
  }
}
