using System;
using System.Collections.Generic;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserSentMessageAction
  {
    public UserSentMessageAction(string type, UserSentMessageActionDto message, IList<MessageDto> messages, IList<int> usersId)
    {
      Type = type;
      Message = message;
      Messages = messages;
      UsersId = usersId;
    }

    public string Type { get; private set; }
    public UserSentMessageActionDto Message { get; private set; }
    public IList<MessageDto> Messages { get; private set; }
    public IList<int> UsersId { get; private set; }
  }

  public class UserSentMessageActionDto
  {
    public UserSentMessageActionDto(int messageId, string type, DateTimeOffset date, string text, string attachmentUrl, string action, int userId, string userName, int groupId)
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
