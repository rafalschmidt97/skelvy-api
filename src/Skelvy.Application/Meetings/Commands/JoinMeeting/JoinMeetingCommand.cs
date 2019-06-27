using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.JoinMeeting
{
  public class JoinMeetingCommand : ICommand
  {
    public JoinMeetingCommand(int userId, int meetingId)
    {
      UserId = userId;
      MeetingId = meetingId;
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
  }
}
