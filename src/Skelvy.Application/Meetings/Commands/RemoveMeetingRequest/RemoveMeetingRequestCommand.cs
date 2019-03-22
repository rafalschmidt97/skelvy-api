using MediatR;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommand : IRequest
  {
    public int UserId { get; set; }
  }
}
