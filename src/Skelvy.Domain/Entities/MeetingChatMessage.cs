using System;

namespace Skelvy.Domain.Entities
{
  public class MeetingChatMessage
  {
    public MeetingChatMessage(string message, DateTimeOffset date, int? attachmentId, int userId, int meetingId)
    {
      Message = message;
      Date = date;
      AttachmentId = attachmentId;
      UserId = userId;
      MeetingId = meetingId;
    }

    public MeetingChatMessage(int id, string message, DateTimeOffset date, int? attachmentId, int userId, int meetingId, User user, Meeting meeting, Attachment attachment)
    {
      Id = id;
      Message = message;
      Date = date;
      AttachmentId = attachmentId;
      UserId = userId;
      MeetingId = meetingId;
      User = user;
      Meeting = meeting;
      Attachment = attachment;
    }

    public int Id { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public int? AttachmentId { get; private set; }
    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
    public User User { get; private set; }
    public Meeting Meeting { get; private set; }
    public Attachment Attachment { get; private set; }
  }
}
