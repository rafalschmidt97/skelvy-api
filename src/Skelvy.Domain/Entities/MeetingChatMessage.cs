using System;

namespace Skelvy.Domain.Entities
{
  public class MeetingChatMessage
  {
    public MeetingChatMessage(string message, DateTimeOffset date, int? attachmentId, int userId, int groupId)
    {
      Message = message;
      Date = date;
      AttachmentId = attachmentId;
      UserId = userId;
      GroupId = groupId;
    }

    public MeetingChatMessage(int id, string message, DateTimeOffset date, int? attachmentId, int userId, int groupId, User user, Group group, Attachment attachment)
    {
      Id = id;
      Message = message;
      Date = date;
      AttachmentId = attachmentId;
      UserId = userId;
      GroupId = groupId;
      User = user;
      Group = group;
      Attachment = attachment;
    }

    public int Id { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public int? AttachmentId { get; private set; }
    public int UserId { get; private set; }
    public int GroupId { get; private set; }

    public User User { get; private set; }
    public Group Group { get; private set; }
    public Attachment Attachment { get; private set; }
  }
}
