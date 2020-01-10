using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class FriendInvitationsRepository : BaseRepository, IFriendInvitationsRepository
  {
    public FriendInvitationsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<FriendInvitation>> FindAllWithInvitingDetailsByUserId(int userId)
    {
      var requests = await Context.FriendInvitations
        .Include(x => x.InvitingUser)
        .ThenInclude(x => x.Profile)
        .Where(r => r.InvitedUserId == userId && !r.IsRemoved)
        .ToListAsync();

      foreach (var request in requests)
      {
        request.InvitingUser.Profile.Photos = await Context.ProfilePhotos
          .Include(x => x.Attachment)
          .Where(x => x.ProfileId == request.InvitingUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();
      }

      return requests;
    }

    public async Task<FriendInvitation> FindOneByInvitationId(int invitationId)
    {
      return await Context.FriendInvitations
        .FirstOrDefaultAsync(r => r.Id == invitationId && !r.IsRemoved);
    }

    public async Task<FriendInvitation> FindOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId)
    {
      return await Context.FriendInvitations.FirstOrDefaultAsync(
        x => ((x.InvitingUserId == invitingUserId && x.InvitedUserId == invitedUserId) ||
              (x.InvitingUserId == invitedUserId && x.InvitedUserId == invitingUserId)) &&
             !x.IsRemoved);
    }

    public async Task<bool> ExistsOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId)
    {
      return await Context.FriendInvitations.AnyAsync(
        x => ((x.InvitingUserId == invitingUserId && x.InvitedUserId == invitedUserId) ||
              (x.InvitingUserId == invitedUserId && x.InvitedUserId == invitingUserId)) &&
             !x.IsRemoved);
    }

    public async Task<IList<FriendInvitation>> FindAllByUserId(int userId)
    {
      return await Context.FriendInvitations
        .Where(x => (x.InvitedUserId == userId || x.InvitingUserId == userId) && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<FriendInvitation>> FindAllWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.FriendInvitations
        .Where(x => usersId.Any(y => y == x.InvitedUserId) || usersId.Any(y => y == x.InvitingUserId))
        .ToListAsync();
    }

    public async Task Add(FriendInvitation invitations)
    {
      await Context.FriendInvitations.AddAsync(invitations);
      await Context.SaveChangesAsync();
    }

    public async Task Update(FriendInvitation invitation)
    {
      Context.FriendInvitations.Update(invitation);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateRange(IList<FriendInvitation> invitations)
    {
      Context.FriendInvitations.UpdateRange(invitations);
      await Context.SaveChangesAsync();
    }

    public async Task RemoveRange(IList<FriendInvitation> invitations)
    {
      Context.FriendInvitations.RemoveRange(invitations);
      await Context.SaveChangesAsync();
    }
  }
}
