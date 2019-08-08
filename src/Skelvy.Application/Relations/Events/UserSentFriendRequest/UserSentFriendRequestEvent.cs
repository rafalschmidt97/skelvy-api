using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Events.UserSentFriendRequest
{
  public class UserSentFriendRequestEvent : IEvent
  {
    public UserSentFriendRequestEvent(int requestId, int invitingUserId, int invitedUserId)
    {
      RequestId = requestId;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
    }

    public int RequestId { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
  }
}
