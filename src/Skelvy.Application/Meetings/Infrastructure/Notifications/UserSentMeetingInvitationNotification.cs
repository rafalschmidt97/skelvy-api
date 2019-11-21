using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserSentMeetingInvitationNotification
  {
    public UserSentMeetingInvitationNotification(int requestId, IEnumerable<int> usersId)
    {
      InvitationId = requestId;
      UsersId = usersId;
    }

    public int InvitationId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
