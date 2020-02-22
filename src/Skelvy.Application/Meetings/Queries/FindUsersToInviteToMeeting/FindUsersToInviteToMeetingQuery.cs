using System.Collections.Generic;
using Newtonsoft.Json;
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

    [JsonConstructor]
    public FindUsersToInviteToMeetingQuery()
    {
    }

    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
