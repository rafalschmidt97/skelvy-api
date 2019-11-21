using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class MeetingAbortedNotification
  {
    public MeetingAbortedNotification(int meetingId, int groupId, int userId, IEnumerable<int> usersId)
    {
      MeetingId = meetingId;
      GroupId = groupId;
      UserId = userId;
      UsersId = usersId;
    }

    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public int UserId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
