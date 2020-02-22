using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public DbSet<RefreshToken> RefreshTokens { get; private set; }
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
    public DbSet<FriendInvitation> FriendInvitations { get; private set; }
    public DbSet<MeetingInvitation> MeetingInvitations { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkelvyContext).Assembly);
      SqlLiteDateConvert(modelBuilder);
    }

    private void SqlLiteDateConvert(ModelBuilder builder)
    {
      if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
      {
        // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
        // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
        // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
        // use the DateTimeOffsetToBinaryConverter
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
          var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                         || p.PropertyType == typeof(DateTimeOffset?));
          foreach (var property in properties)
          {
            builder
              .Entity(entityType.Name)
              .Property(property.Name)
              .HasConversion(new DateTimeOffsetToBinaryConverter());
          }
        }
      }
    }
  }
}
