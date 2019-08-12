using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserJoinedGroup
{
  public class UserJoinedGroupEvent : IEvent
  {
    public UserJoinedGroupEvent(int userId, int groupId)
    {
      UserId = userId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
