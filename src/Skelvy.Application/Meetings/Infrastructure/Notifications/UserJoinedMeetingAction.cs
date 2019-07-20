using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserJoinedMeetingAction
  {
    public UserJoinedMeetingAction(int userId, IEnumerable<int> usersId)
    {
      UserId = userId;
      UsersId = usersId;
    }

    public int UserId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
