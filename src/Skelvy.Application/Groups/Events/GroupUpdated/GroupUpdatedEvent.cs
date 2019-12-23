using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Events.GroupUpdated
{
  public class GroupUpdatedEvent : IEvent
  {
    public GroupUpdatedEvent(int userId, int groupId)
    {
      UserId = userId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
