using System;
using System.Collections.Generic;
using Skelvy.Application.Messages.Queries;

namespace Skelvy.Application.Messages.Infrastructure.Notifications
{
  public class UserSentMessageNotification
  {
    public UserSentMessageNotification(string type, int groupId, UserSentMessageNotificationDto message, IList<MessageDto> messages, IList<int> usersId)
    {
      Type = type;
      GroupId = groupId;
      Message = message;
      Messages = messages;
      UsersId = usersId;
    }

    public string Type { get; private set; }
    public int GroupId { get; private set; }
    public UserSentMessageNotificationDto Message { get; private set; }
    public IList<MessageDto> Messages { get; private set; }
    public IList<int> UsersId { get; private set; }
  }

  public class UserSentMessageNotificationDto
  {
    public UserSentMessageNotificationDto(int messageId, string type, DateTimeOffset date, string text, string attachmentUrl, string action, int userId, string userName, int groupId)
    {
      MessageId = messageId;
      Type = type;
      Date = date;
      Text = text;
      AttachmentUrl = attachmentUrl;
      Action = action;
      UserId = userId;
      UserName = userName;
      GroupId = groupId;
    }

    public int MessageId { get; private set; }
    public string Type { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string Text { get; private set; }
    public string AttachmentUrl { get; private set; }
    public string Action { get; private set; }
    public int UserId { get; private set; }
    public string UserName { get; private set; }
    public int GroupId { get; private set; }
  }
}
