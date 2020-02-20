using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Events.UserRemovedFromGroup
{
  public class UserRemovedFromGroupEvent : IEvent
  {
    public UserRemovedFromGroupEvent(int userId, int removedUserId, int groupId)
    {
      UserId = userId;
      RemovedUserId = removedUserId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int RemovedUserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
