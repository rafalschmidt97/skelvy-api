using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class MeetingUserRoleUpdatedNotification
  {
    public MeetingUserRoleUpdatedNotification(int meetingId, int groupId, int userId, int updatedUserId, string role, IEnumerable<int> usersId)
    {
      MeetingId = meetingId;
      GroupId = groupId;
      UserId = userId;
      UpdatedUserId = updatedUserId;
      Role = role;
      UsersId = usersId;
    }

    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public int UserId { get; private set; }
    public int UpdatedUserId { get; private set; }
    public string Role { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
