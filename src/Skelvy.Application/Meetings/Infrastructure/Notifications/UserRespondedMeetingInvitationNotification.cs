using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserRespondedMeetingInvitationNotification
  {
    public UserRespondedMeetingInvitationNotification(int invitationId, bool isAccepted, int invitingUserId, int invitedUserId, int meetingId, int groupId, IEnumerable<int> usersId)
    {
      InvitationId = invitationId;
      IsAccepted = isAccepted;
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
      MeetingId = meetingId;
      GroupId = groupId;
      UsersId = usersId;
    }

    public int InvitationId { get; private set; }
    public bool IsAccepted { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
