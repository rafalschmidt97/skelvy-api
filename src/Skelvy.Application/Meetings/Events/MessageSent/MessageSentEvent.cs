using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Events.MessageSent
{
  public class MessageSentEvent : IEvent
  {
    public MessageSentEvent(MessageSentEventDto message, IList<MessageDto> messages)
    {
      Message = message;
      Messages = messages;
    }

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
