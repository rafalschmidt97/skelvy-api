using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserRespondedMeetingInvitation
{
  public class UserRespondedMeetingInvitationEvent : IEvent
  {
    public UserRespondedMeetingInvitationEvent(int invitationId, bool isAccepted, int invitingUserId, int invitedUserId)
    {
      InvitationId = invitationId;
      IsAccepted = isAccepted;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
    }

    public int InvitationId { get; private set; }
    public bool IsAccepted { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
  }
}
