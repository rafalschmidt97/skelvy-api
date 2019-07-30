using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Commands.AddMessage
{
  public class AddMessageCommand : ICommandData<MessageDto>
  {
    public AddMessageCommand(int groupId, string text, string attachmentUrl, int userId)
    {
      GroupId = groupId;
      Text = text;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
    }

    public int GroupId { get; set; }
    public string Text { get; set; }
    public string AttachmentUrl { get; set; }
    public int UserId { get; set; }
  }
}
