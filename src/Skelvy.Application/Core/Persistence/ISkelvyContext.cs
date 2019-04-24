using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Core.Persistence
{
  public interface ISkelvyContext
  {
    DbSet<User> Users { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<UserProfilePhoto> UserProfilePhotos { get; }
    DbSet<Drink> Drinks { get; }
    DbSet<MeetingRequest> MeetingRequests { get; }
    DbSet<MeetingRequestDrink> MeetingRequestDrinks { get; }
    DbSet<Meeting> Meetings { get; }
    DbSet<MeetingUser> MeetingUsers { get; }
    DbSet<MeetingChatMessage> MeetingChatMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    int SaveChanges();
  }
}
