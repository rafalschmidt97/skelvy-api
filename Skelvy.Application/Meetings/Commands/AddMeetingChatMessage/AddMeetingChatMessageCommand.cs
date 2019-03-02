using MediatR;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommand : IRequest
  {
    public string Message { get; set; }
    public int UserId { get; set; }
  }
}
