using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Messages.Queries;

namespace Skelvy.Application.Messages.Commands.AddMessage
{
  public class AddMessageCommand : ICommandData<IList<MessageDto>>
  {
    public AddMessageCommand(string type, string text, string attachmentUrl, string action, int userId, int groupId)
    {
      Type = type;
      Text = text;
      AttachmentUrl = attachmentUrl;
      Action = action;
      UserId = userId;
      GroupId = groupId;
    }

    [JsonConstructor]
    public AddMessageCommand()
    {
    }

    public string Type { get; set; }
    public string Text { get; set; }
    public string AttachmentUrl { get; set; }
    public string Action { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
  }
}
