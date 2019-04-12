using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommand : ICommand
  {
    public int UserId { get; set; }
  }
}
