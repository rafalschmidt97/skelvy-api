using System;
using MediatR;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommand : IRequest
  {
    public DateTimeOffset Date { get; set; }
    public string Message { get; set; }
    public int UserId { get; set; }
  }
}
