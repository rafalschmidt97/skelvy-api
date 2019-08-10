using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserFoundMeetingNotification
  {
    public UserFoundMeetingNotification(IEnumerable<int> usersId)
    {
      UsersId = usersId;
    }

    public IEnumerable<int> UsersId { get; private set; }
  }
}
