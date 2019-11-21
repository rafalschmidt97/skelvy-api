using System.Collections.Generic;

namespace Skelvy.Application.Groups.Infrastructure.Notifications
{
  public class GroupAbortedNotification
  {
    public GroupAbortedNotification(int groupId, int userId, IEnumerable<int> usersId)
    {
      GroupId = groupId;
      UserId = userId;
      UsersId = usersId;
    }

    public int GroupId { get; private set; }
    public int UserId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
