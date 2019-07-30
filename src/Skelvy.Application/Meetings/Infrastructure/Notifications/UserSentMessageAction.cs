using System;
using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserSentMessageAction
  {
    public UserSentMessageAction(int messageId, string text, DateTimeOffset date, string attachmentUrl, int userId, string userName, int meetingId, IEnumerable<int> usersId)
    {
      MessageId = messageId;
      Text = text;
      Date = date;
      AttachmentUrl = attachmentUrl;
      UserId = userId;
      UserName = userName;
      MeetingId = meetingId;
      UsersId = usersId;
    }

    public int MessageId { get; private set; }
    public string Text { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string AttachmentUrl { get; private set; }
    public int UserId { get; private set; }
    public string UserName { get; private set; }
    public int MeetingId { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}
