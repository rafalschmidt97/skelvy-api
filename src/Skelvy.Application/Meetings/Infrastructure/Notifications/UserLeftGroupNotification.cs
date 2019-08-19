using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserLeftGroupNotification
  {
    public UserLeftGroupNotification(int userId, int groupId, IEnumerable<int> usersId)
    {
      UserId = userId;
      GroupId = groupId;
      UsersId = usersId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
