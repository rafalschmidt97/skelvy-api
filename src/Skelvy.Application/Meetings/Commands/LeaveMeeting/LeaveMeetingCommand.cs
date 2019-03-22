using MediatR;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommand : IRequest
  {
    public int UserId { get; set; }
  }
}
