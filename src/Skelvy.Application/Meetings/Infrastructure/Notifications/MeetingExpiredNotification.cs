using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class MeetingExpiredNotification
  {
    public MeetingExpiredNotification(int meetingId, int groupId, IEnumerable<int> usersId)
    {
      MeetingId = meetingId;
      GroupId = groupId;
      UsersId = usersId;
    }

    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
