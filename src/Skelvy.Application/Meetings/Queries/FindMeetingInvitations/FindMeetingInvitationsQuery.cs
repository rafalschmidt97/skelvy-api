using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitations
{
  public class FindMeetingInvitationsQuery : IQuery<IList<SelfMeetingInvitationDto>>
  {
    public FindMeetingInvitationsQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
