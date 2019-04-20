using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommand : ICommand
  {
    public AddMeetingChatMessageCommand(DateTimeOffset date, string message, int userId)
    {
      Date = date;
      Message = message;
      UserId = userId;
    }

    public DateTimeOffset Date { get; set; }
    public string Message { get; set; }
    public int UserId { get; set; }
  }
}
