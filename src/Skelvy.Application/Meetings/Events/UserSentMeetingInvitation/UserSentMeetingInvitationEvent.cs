using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserSentMeetingInvitation
{
  public class UserSentMeetingInvitationEvent : IEvent
  {
    public UserSentMeetingInvitationEvent(int invitationId, int invitingUserId, int invitedUserId, int meetingId)
    {
      InvitationId = invitationId;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
      MeetingId = meetingId;
    }

    public int InvitationId { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
