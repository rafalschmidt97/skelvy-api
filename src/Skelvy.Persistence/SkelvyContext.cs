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
    public DbSet<Profile> Profiles { get; private set; }
    public DbSet<ProfilePhoto> ProfilePhotos { get; private set; }
    public DbSet<Activity> Activities { get; private set; }
    public DbSet<MeetingRequest> MeetingRequests { get; private set; }
    public DbSet<MeetingRequestActivity> MeetingRequestActivities { get; private set; }
    public DbSet<Meeting> Meetings { get; private set; }
    public DbSet<Group> Groups { get; private set; }
    public DbSet<GroupUser> GroupUsers { get; private set; }
    public DbSet<Message> Messages { get; private set; }
    public DbSet<Attachment> Attachments { get; private set; }
    public DbSet<Relation> Relations { get; private set; }
    public DbSet<FriendRequest> FriendRequests { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkelvyContext).Assembly);
    }
  }
}
