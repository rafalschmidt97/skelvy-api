using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommand : ICommandData<MeetingChatMessageDto>
  {
    public AddMeetingChatMessageCommand(string message, string attachmentUrl, int userId)
    {
      Message = message;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
    }

    public string Message { get; set; }
    public string AttachmentUrl { get; set; }
    public int UserId { get; set; }
  }
}
