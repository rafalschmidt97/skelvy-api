using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingInvitationsRepository : IBaseRepository
  {
    Task<bool> ExistsOneByInvitedUserIdAndMeetingId(int invitedUserId, int meetingId);
    Task<IList<MeetingInvitation>> FindAllWithInvitingDetailsByUserId(int userId);
    Task<IList<MeetingInvitation>> FindAllByMeetingId(int meetingId);
    Task<MeetingInvitation> FindOneByRequestId(int requestId);
    Task<bool> ExistsOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId);
    Task<IList<MeetingInvitation>> FindAllWithRemovedByUsersId(List<int> usersId);
    Task<IList<MeetingInvitation>> FindAllByMeetingsId(List<int> meetingsId);
    Task Add(MeetingInvitation friendsRequest);
    Task Update(MeetingInvitation request);
    Task UpdateRange(IList<MeetingInvitation> invitations);
    Task RemoveRange(IList<MeetingInvitation> invitations);
  }
}
