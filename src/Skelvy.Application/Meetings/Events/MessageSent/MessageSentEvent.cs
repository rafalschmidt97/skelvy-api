using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MessageSent
{
  public class MessageSentEvent : IEvent
  {
    public MessageSentEvent(int messageId, string text, DateTimeOffset date, string attachmentUrl, int userId, int groupId)
    {
      MessageId = messageId;
      Text = text;
      Date = date;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
      GroupId = groupId;
    }

    public int MessageId { get; private set; }
    public string Text { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string AttachmentUrl { get; private set; }
    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
