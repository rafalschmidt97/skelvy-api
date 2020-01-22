using System.Collections.Generic;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Meetings.Queries.FindUsersToInviteToMeeting
{
  public class FindUsersToInviteToMeetingQuery : IQuery<IList<UserDto>>
  {
    public FindUsersToInviteToMeetingQuery(int meetingId, int userId, int page)
    {
      MeetingId = meetingId;
      UserId = userId;
      Page = page;
    }

    public FindUsersToInviteToMeetingQuery() // required for FromQuery attribute
    {
    }

    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
