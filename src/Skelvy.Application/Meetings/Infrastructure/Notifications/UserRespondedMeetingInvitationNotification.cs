using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserRespondedMeetingInvitationNotification
  {
    public UserRespondedMeetingInvitationNotification(int invitationId, bool isAccepted, IEnumerable<int> usersId)
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
