using System;

namespace Skelvy.Domain.Entities
{
  public class Message
  {
    public Message(DateTimeOffset date, string text, int? attachmentId, int userId, int groupId)
    {
      Date = date;
      Text = text;
      AttachmentId = attachmentId;
      UserId = userId;
      GroupId = groupId;
    }

    public Message(int id, DateTimeOffset date, string text, int? attachmentId, int userId, int groupId, User user, Group group, Attachment attachment)
    {
      Id = id;
      Date = date;
      Text = text;
      AttachmentId = attachmentId;
      UserId = userId;
      GroupId = groupId;
      User = user;
      Group = group;
      Attachment = attachment;
    }

    public int Id { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string Text { get; private set; }
    public int? AttachmentId { get; private set; }
    public int UserId { get; private set; }
    public int GroupId { get; private set; }

    public User User { get; private set; }
    public Group Group { get; private set; }
    public Attachment Attachment { get; private set; }
  }
}
