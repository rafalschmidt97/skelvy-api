using MediatR;

namespace Skelvy.Application.Meetings.Commands.DeleteMeetingRequest
{
  public class DeleteMeetingRequestCommand : IRequest
  {
    public int UserId { get; set; }
  }
}
