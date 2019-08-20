using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserRespondedMeetingInvitationNotification
  {
    public UserRespondedMeetingInvitationNotification(int requestId, bool isAccepted, IEnumerable<int> usersId)
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
