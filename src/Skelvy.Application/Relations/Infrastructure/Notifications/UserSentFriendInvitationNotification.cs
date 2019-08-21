using System.Collections.Generic;

namespace Skelvy.Application.Relations.Infrastructure.Notifications
{
  public class UserSentFriendInvitationNotification
  {
    public UserSentFriendInvitationNotification(int invitationId, IEnumerable<int> usersId)
    {
      InvitationId = invitationId;
      UsersId = usersId;
    }

    public int InvitationId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
