using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommand : ICommand
  {
    public LeaveMeetingCommand(int meetingId, int userId)
    {
      MeetingId = meetingId;
      UserId = userId;
    }

    [JsonConstructor]
    public LeaveMeetingCommand()
    {
    }

    public int MeetingId { get; set; }
    public int UserId { get; set; }
  }
}
