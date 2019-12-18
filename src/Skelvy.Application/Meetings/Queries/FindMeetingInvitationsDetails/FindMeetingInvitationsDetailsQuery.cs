using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitationsDetails
{
  public class FindMeetingInvitationsDetailsQuery : IQuery<IList<MeetingInvitationDto>>
  {
    public FindMeetingInvitationsDetailsQuery(int meetingId, int userId)
    {
      MeetingId = meetingId;
      UserId = userId;
    }

    public int MeetingId { get; set; }
    public int UserId { get; set; }
  }
}
