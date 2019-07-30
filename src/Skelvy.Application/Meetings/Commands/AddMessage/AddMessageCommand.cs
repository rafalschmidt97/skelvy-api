using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Commands.AddMessage
{
  public class AddMessageCommand : ICommandData<MessageDto>
  {
    public AddMessageCommand(string message, string attachmentUrl, int userId)
    {
      Message = message;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
    }

    public string Message { get; set; }
    public string Text { get; set; }
    public string AttachmentUrl { get; set; }
    public int UserId { get; set; }
  }
}
