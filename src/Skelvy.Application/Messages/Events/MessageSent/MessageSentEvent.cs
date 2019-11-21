using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Messages.Queries;

namespace Skelvy.Application.Messages.Events.MessageSent
{
  public class MessageSentEvent : IEvent
  {
    public MessageSentEvent(string type, int groupId, MessageSentEventDto message, IList<MessageDto> messages)
    {
      Type = type;
      GroupId = groupId;
      Message = message;
      Messages = messages;
    }

    public string Type { get; private set; }
    public int GroupId { get; private set; }
    public MessageSentEventDto Message { get; private set; }
    public IList<MessageDto> Messages { get; private set; }
  }

  public class MessageSentEventDto : IEvent
  {
    public MessageSentEventDto(int messageId, string type, DateTimeOffset date, string text, string attachmentUrl, string action, int userId, int groupId)
    {
      MessageId = messageId;
      Type = type;
      Date = date;
      Text = text;
      AttachmentUrl = attachmentUrl;
      Action = action;
      UserId = userId;
      GroupId = groupId;
    }

    public int MessageId { get; private set; }
    public string Type { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string Text { get; private set; }
    public string AttachmentUrl { get; private set; }
    public string Action { get; private set; }
    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
