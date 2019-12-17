using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserRespondedMeetingInvitation
{
  public class UserRespondedMeetingInvitationEvent : IEvent
  {
    public UserRespondedMeetingInvitationEvent(int invitationId, bool isAccepted, int invitingUserId, int invitedUserId, int meetingId, int groupId)
    {
      InvitationId = invitationId;
      IsAccepted = isAccepted;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
      MeetingId = meetingId;
      GroupId = groupId;
    }

    public int InvitationId { get; private set; }
    public bool IsAccepted { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
  }
}
