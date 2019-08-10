using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class MeetingRequestExpiredNotification
  {
    public MeetingRequestExpiredNotification(IEnumerable<int> usersId)
    {
      UsersId = usersId;
    }

    public IEnumerable<int> UsersId { get; private set; }
  }
}
