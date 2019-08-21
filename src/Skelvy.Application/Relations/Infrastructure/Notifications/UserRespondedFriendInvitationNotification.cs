using System.Collections.Generic;

namespace Skelvy.Application.Relations.Infrastructure.Notifications
{
  public class UserRespondedFriendInvitationNotification
  {
    public UserRespondedFriendInvitationNotification(int invitationId, bool isAccepted, IEnumerable<int> usersId)
    {
      InvitationId = invitationId;
      IsAccepted = isAccepted;
      UsersId = usersId;
    }

    public int InvitationId { get; private set; }
    public bool IsAccepted { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
