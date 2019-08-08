using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Events.UserRespondedFriendRequest
{
  public class UserRespondedFriendRequestEvent : IEvent
  {
    public UserRespondedFriendRequestEvent(int requestId, bool isAccepted, int invitingUserId, int invitedUserId)
    {
      RequestId = requestId;
      IsAccepted = isAccepted;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
    }

    public int RequestId { get; private set; }
    public bool IsAccepted { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
  }
}
