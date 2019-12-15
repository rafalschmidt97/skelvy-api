using System.Collections.Generic;

namespace Skelvy.Application.Relations.Infrastructure.Notifications
{
  public class FriendRemovedNotification
  {
    public FriendRemovedNotification(int removingUserId, int removedUserId, IEnumerable<int> usersId)
    {
      RemovingUserId = removingUserId;
      RemovedUserId = removedUserId;
      UsersId = usersId;
    }

    public int RemovingUserId { get; private set; }
    public int RemovedUserId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
