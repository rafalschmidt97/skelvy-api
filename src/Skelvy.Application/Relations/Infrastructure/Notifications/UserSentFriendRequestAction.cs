using System.Collections.Generic;

namespace Skelvy.Application.Relations.Infrastructure.Notifications
{
  public class UserSentFriendRequestAction
  {
    public UserSentFriendRequestAction(int requestId, IEnumerable<int> usersId)
    {
      RequestId = requestId;
      UsersId = usersId;
    }

    public int RequestId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
