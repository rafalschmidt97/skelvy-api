using System;
using Skelvy.Domain.Enums;
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

    public int Id { get; set; }
    public string Type { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Text { get; set; }
    public int? AttachmentId { get; set; }
    public string Action { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }

    public User User { get; set; }
    public Group Group { get; set; }
    public Attachment Attachment { get; set; }

    public void UpdateSeen()
    {
      if (Type == MessageType.Action && Action == MessageActionType.Seen)
      {
        Date = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException(
          $"{nameof(Message)}({Id}) is not {MessageActionType.Seen} or type is not {MessageType.Action}.");
      }
    }
  }
}
