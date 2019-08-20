using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.InviteToMeeting
{
  public class InviteToMeetingCommand : ICommand
  {
    public InviteToMeetingCommand(int userId, int invitedUserId, int meetingId)
    {
      UserId = userId;
      InvitedUserId = invitedUserId;
      MeetingId = meetingId;
    }

    public int UserId { get; set; }
    public int InvitedUserId { get; set; }
    public int MeetingId { get; set; }
  }
}
