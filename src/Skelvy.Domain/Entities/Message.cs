using System;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Message
  {
    public Message(string type, DateTimeOffset date, string text, int? attachmentId, string action, int userId, int groupId)
    {
      Type = type;
      Date = date;
      Text = text;
      AttachmentId = attachmentId;
      Action = action;
      UserId = userId;
      GroupId = groupId;
    }

    public Message(int id, string type, DateTimeOffset date, string text, int? attachmentId, string action, int userId, int groupId, User user, Group group, Attachment attachment)
    {
      Id = id;
      Type = type;
      Date = date;
      Text = text;
      AttachmentId = attachmentId;
      Action = action;
      UserId = userId;
      GroupId = groupId;
      User = user;
      Group = group;
      Attachment = attachment;
    }

    public int Id { get; private set; }
    public string Type { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string Text { get; private set; }
    public int? AttachmentId { get; private set; }
    public string Action { get; private set; }
    public int UserId { get; private set; }
    public int GroupId { get; private set; }

    public User User { get; private set; }
    public Group Group { get; private set; }
    public Attachment Attachment { get; private set; }

    public void UpdateSeen()
    {
      if (Type == MessageTypes.Action && Action == MessageActionTypes.Seen)
      {
        Date = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException(
          $"Entity {nameof(Message)}(Id = {Id}) is not {MessageActionTypes.Seen} or type is not {MessageTypes.Action}.");
      }
    }
  }
}
