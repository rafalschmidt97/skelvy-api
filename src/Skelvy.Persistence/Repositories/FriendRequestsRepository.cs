using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class FriendRequestsRepository : BaseRepository, IFriendRequestsRepository
  {
    public FriendRequestsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<FriendRequest>> FindAllWithInvitingDetailsByUserId(int userId)
    {
      var requests = await Context.FriendRequests
        .Include(x => x.InvitingUser)
        .ThenInclude(x => x.Profile)
        .Where(r => r.InvitedUserId == userId && !r.IsRemoved)
        .ToListAsync();

      foreach (var request in requests)
      {
        request.InvitingUser.Profile.Photos = await Context.ProfilePhotos
          .Where(x => x.ProfileId == request.InvitingUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();
      }

      return requests;
    }

    public async Task<FriendRequest> FindOneByRequestId(int requestId)
    {
      return await Context.FriendRequests
        .FirstOrDefaultAsync(r => r.Id == requestId && !r.IsRemoved);
    }

    public async Task<bool> ExistsOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId)
    {
      return await Context.FriendRequests.AnyAsync(
        x => ((x.InvitingUserId == invitingUserId && x.InvitedUserId == invitedUserId) ||
              (x.InvitingUserId == invitedUserId && x.InvitedUserId == invitingUserId)) &&
             !x.IsRemoved);
    }

    public async Task<IList<FriendRequest>> FindAllWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.FriendRequests
        .Where(x => usersId.Any(y => y == x.InvitedUserId || y == x.InvitingUserId))
        .ToListAsync();
    }

    public async Task Add(FriendRequest friendsRequest)
    {
      await Context.FriendRequests.AddAsync(friendsRequest);
      await Context.SaveChangesAsync();
    }

    public async Task Update(FriendRequest request)
    {
      Context.FriendRequests.Update(request);
      await Context.SaveChangesAsync();
    }

    public async Task RemoveRange(IList<FriendRequest> friendRequests)
    {
      Context.FriendRequests.RemoveRange(friendRequests);
      await Context.SaveChangesAsync();
    }
  }
}
