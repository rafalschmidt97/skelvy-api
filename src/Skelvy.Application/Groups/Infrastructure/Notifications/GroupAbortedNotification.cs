using System.Collections.Generic;

namespace Skelvy.Application.Groups.Infrastructure.Notifications
{
  public class GroupAbortedNotification
  {
    public GroupAbortedNotification(int userId, IEnumerable<int> usersId)
    {
      UserId = userId;
      UsersId = usersId;
    }

    public int UserId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
