using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserConnectedToMeetingNotification
  {
    public UserConnectedToMeetingNotification(int meetingId, int requestId, int groupId, IEnumerable<int> usersId)
    {
      MeetingId = meetingId;
      RequestId = requestId;
      GroupId = groupId;
      UsersId = usersId;
    }

    public int MeetingId { get; private set; }
    public int RequestId { get; private set; }
    public int GroupId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
