using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Events.UserSentFriendInvitation
{
  public class UserSentFriendInvitationEvent : IEvent
  {
    public UserSentFriendInvitationEvent(int invitationId, int invitingUserId, int invitedUserId)
    {
      InvitationId = invitationId;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
    }

    public int InvitationId { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
  }
}
