using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class MeetingRequestExpiredNotification
  {
    public MeetingRequestExpiredNotification(int requestId, IEnumerable<int> usersId)
    {
      RequestId = requestId;
      UsersId = usersId;
    }

    public int RequestId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
