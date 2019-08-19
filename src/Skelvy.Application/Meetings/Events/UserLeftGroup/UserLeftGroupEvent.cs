using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserLeftGroup
{
  public class UserLeftGroupEvent : IEvent
  {
    public UserLeftGroupEvent(int userId, int groupId)
    {
      UserId = userId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
