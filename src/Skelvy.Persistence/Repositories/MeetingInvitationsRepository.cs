using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingInvitationsRepository : BaseRepository, IMeetingInvitationsRepository
  {
    public MeetingInvitationsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<bool> ExistsOneByInvitedUserIdAndMeetingId(int invitedUserId, int meetingId)
    {
      return await Context.MeetingInvitations
        .AnyAsync(x => x.InvitedUserId == invitedUserId && x.MeetingId == meetingId && !x.IsRemoved);
    }

    public async Task<IList<MeetingInvitation>> FindAllWithInvitingDetailsByUserId(int userId)
    {
      var requests = await Context.MeetingInvitations
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

    public async Task<IList<MeetingInvitation>> FindAllByMeetingId(int meetingId)
    {
      return await Context.MeetingInvitations
        .Where(x => x.MeetingId == meetingId && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<MeetingInvitation> FindOneByRequestId(int requestId)
    {
      return await Context.MeetingInvitations
        .FirstOrDefaultAsync(x => x.Id == requestId && !x.IsRemoved);
    }

    public async Task<bool> ExistsOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId)
    {
      return await Context.MeetingInvitations.AnyAsync(
        x => ((x.InvitingUserId == invitingUserId && x.InvitedUserId == invitedUserId) ||
              (x.InvitingUserId == invitedUserId && x.InvitedUserId == invitingUserId)) &&
             !x.IsRemoved);
    }

    public async Task<IList<MeetingInvitation>> FindAllWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.MeetingInvitations
        .Where(x => usersId.Any(y => y == x.InvitedUserId || y == x.InvitingUserId))
        .ToListAsync();
    }

    public async Task<IList<MeetingInvitation>> FindAllByMeetingsId(List<int> meetingsId)
    {
      return await Context.MeetingInvitations
        .Where(x => meetingsId.Any(y => y == x.Id) && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task Add(MeetingInvitation friendsRequest)
    {
      await Context.MeetingInvitations.AddAsync(friendsRequest);
      await Context.SaveChangesAsync();
    }

    public async Task Update(MeetingInvitation request)
    {
      Context.MeetingInvitations.Update(request);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateRange(IList<MeetingInvitation> invitations)
    {
      Context.MeetingInvitations.UpdateRange(invitations);
      await Context.SaveChangesAsync();
    }

    public async Task RemoveRange(IList<MeetingInvitation> invitations)
    {
      Context.MeetingInvitations.RemoveRange(invitations);
      await Context.SaveChangesAsync();
    }
  }
}
