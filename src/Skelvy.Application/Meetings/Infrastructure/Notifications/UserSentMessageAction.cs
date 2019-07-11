using System;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserSentMessageAction
  {
    public UserSentMessageAction(int messageId, string message, DateTimeOffset date, string attachmentUrl, int userId, string userName, int meetingId)
    {
      MessageId = messageId;
      Message = message;
      Date = date;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
      UserName = userName;
      MeetingId = meetingId;
    }

    public int MessageId { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string AttachmentUrl { get; private set; }
    public int UserId { get; private set; }
    public string UserName { get; private set; }
    public int MeetingId { get; private set; }
  }
}
