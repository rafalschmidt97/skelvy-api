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

    public async Task<IList<MeetingInvitation>> FindAllWithActivityAndUsersDetailsByUserId(int userId)
    {
      var meetingInvitations = await Context.MeetingInvitations
        .Include(x => x.Meeting)
        .ThenInclude(x => x.Group)
        .ThenInclude(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Where(x => x.InvitedUserId == userId && !x.IsRemoved)
        .ToListAsync();

      foreach (var meetingInvitation in meetingInvitations)
      {
        foreach (var groupUser in meetingInvitation.Meeting.Group.Users)
        {
          var userPhotos = await Context.ProfilePhotos
            .Include(x => x.Attachment)
            .Where(x => x.ProfileId == groupUser.User.Profile.Id)
            .OrderBy(x => x.Order)
            .ToListAsync();

          groupUser.User.Profile.Photos = userPhotos;
        }
      }

      return meetingInvitations;
    }

    public async Task<IList<MeetingInvitation>> FindAllWithInvitedUserDetailsByMeetingId(int meetingId)
    {
      var meetingInvitations = await Context.MeetingInvitations
        .Include(x => x.InvitedUser)
        .ThenInclude(x => x.Profile)
        .Where(x => x.MeetingId == meetingId && !x.IsRemoved)
        .ToListAsync();

      foreach (var meetingInvitation in meetingInvitations)
      {
        var userPhotos = await Context.ProfilePhotos
          .Include(x => x.Attachment)
          .Where(x => x.ProfileId == meetingInvitation.InvitedUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();

        meetingInvitation.InvitedUser.Profile.Photos = userPhotos;
      }

      return meetingInvitations;
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
