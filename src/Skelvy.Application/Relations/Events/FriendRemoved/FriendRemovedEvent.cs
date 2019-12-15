using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Events.FriendRemoved
{
  public class FriendRemovedEvent : IEvent
  {
    public FriendRemovedEvent(int removingUserId, int removedUserId)
    {
      RemovingUserId = removingUserId;
      RemovedUserId = removedUserId;
    }

    public int RemovingUserId { get; private set; }
    public int RemovedUserId { get; private set; }
  }
}
