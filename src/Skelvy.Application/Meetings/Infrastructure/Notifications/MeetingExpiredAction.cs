using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class MeetingExpiredAction
  {
    public MeetingExpiredAction(IEnumerable<int> usersId)
    {
      UsersId = usersId;
    }

    public IEnumerable<int> UsersId { get; private set; }
  }
}
