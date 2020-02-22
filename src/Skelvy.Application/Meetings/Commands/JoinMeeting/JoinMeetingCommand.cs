using Newtonsoft.Json;
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

    [JsonConstructor]
    public JoinMeetingCommand()
    {
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
  }
}
