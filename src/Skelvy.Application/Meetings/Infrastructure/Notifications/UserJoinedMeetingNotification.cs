using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserJoinedMeetingNotification
  {
    public UserJoinedMeetingNotification(int userId, string userRole, int meetingId, int groupId, IEnumerable<int> usersId)
    {
      UserId = userId;
      UserRole = userRole;
      MeetingId = meetingId;
      GroupId = groupId;
      UsersId = usersId;
    }

    public int UserId { get; private set; }
    public string UserRole { get; set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
