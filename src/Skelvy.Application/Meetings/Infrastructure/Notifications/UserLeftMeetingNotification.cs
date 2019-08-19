using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserLeftMeetingNotification
  {
    public UserLeftMeetingNotification(int userId, int groupId, int meetingId, IEnumerable<int> usersId)
    {
      UserId = userId;
      GroupId = groupId;
      MeetingId = meetingId;
      UsersId = usersId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
    public int MeetingId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
