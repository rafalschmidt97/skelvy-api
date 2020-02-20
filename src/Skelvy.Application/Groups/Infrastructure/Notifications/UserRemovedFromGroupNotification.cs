using System.Collections.Generic;

namespace Skelvy.Application.Groups.Infrastructure.Notifications
{
  public class UserRemovedFromGroupNotification
  {
    public UserRemovedFromGroupNotification(int userId, int removedUserId, int groupId, IEnumerable<int> usersId)
    {
      UserId = userId;
      RemovedUserId = removedUserId;
      GroupId = groupId;
      UsersId = usersId;
    }

    public int UserId { get; private set; }
    public int RemovedUserId { get; private set; }
    public int GroupId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
