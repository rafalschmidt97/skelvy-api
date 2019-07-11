using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommand : ICommand
  {
    public AddMeetingChatMessageCommand(DateTimeOffset date, string message, string attachmentUrl, int userId)
    {
      Date = date;
      Message = message;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
    }

    public DateTimeOffset Date { get; set; }
    public string Message { get; set; }
    public string AttachmentUrl { get; set; }
    public int UserId { get; set; }
  }
}
