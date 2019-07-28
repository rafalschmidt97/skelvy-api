using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MessageSent
{
  public class MessageSentEvent : IEvent
  {
    public MessageSentEvent(int messageId, string message, DateTimeOffset date, string attachmentUrl, int userId, int meetingId)
    {
      MessageId = messageId;
      Message = message;
      Date = date;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
      MeetingId = meetingId;
    }

    public int MessageId { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string AttachmentUrl { get; private set; }
    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
