using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.RemoveMeeting
{
  public class RemoveMeetingCommand : ICommand
  {
    public RemoveMeetingCommand(int meetingId, int userId)
    {
      MeetingId = meetingId;
      UserId = userId;
    }

    [JsonConstructor]
    public RemoveMeetingCommand()
    {
    }

    public int MeetingId { get; set; }
    public int UserId { get; set; }
  }
}
