using System.Collections.Generic;

namespace Skelvy.Application.Relations.Infrastructure.Notifications
{
  public class UserRespondedFriendRequestAction
  {
    public UserRespondedFriendRequestAction(int requestId, bool isAccepted, IEnumerable<int> usersId)
    {
      RequestId = requestId;
      IsAccepted = isAccepted;
      UsersId = usersId;
    }

    public int RequestId { get; private set; }
    public bool IsAccepted { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
