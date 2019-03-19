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
    public DbSet<MeetingRequest> MeetingRequests { get; set; }
    public DbSet<MeetingRequestDrink> MeetingRequestDrinks { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<MeetingUser> MeetingUsers { get; set; }
    public DbSet<MeetingChatMessage> MeetingChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkelvyContext).Assembly);

      // Configuration for many-to-many relationships (automatic joining has not been implemented yet in EF Core)
      modelBuilder.Entity<MeetingRequestDrink>().HasKey(x => new { x.MeetingRequestId, x.DrinkId });
      modelBuilder.Entity<MeetingUser>().HasKey(x => new { x.MeetingId, x.UserId });
    }
  }
}
