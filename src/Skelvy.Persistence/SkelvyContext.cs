using Microsoft.EntityFrameworkCore;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence
{
  public sealed class SkelvyContext : DbContext
  {
    public SkelvyContext(DbContextOptions options)
      : base(options)
    {
      ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<User> Users { get; private set; }
    public DbSet<UserRole> UserRoles { get; private set; }
    public DbSet<UserProfile> UserProfiles { get; private set; }
    public DbSet<UserProfilePhoto> UserProfilePhotos { get; private set; }
    public DbSet<Drink> Drinks { get; private set; }
    public DbSet<MeetingRequest> MeetingRequests { get; private set; }
    public DbSet<MeetingRequestDrink> MeetingRequestDrinks { get; private set; }
    public DbSet<Meeting> Meetings { get; private set; }
    public DbSet<MeetingUser> MeetingUsers { get; private set; }
    public DbSet<MeetingChatMessage> MeetingChatMessages { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkelvyContext).Assembly);
    }
  }
}
