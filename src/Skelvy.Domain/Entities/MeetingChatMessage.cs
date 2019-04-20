using System;

namespace Skelvy.Domain.Entities
{
  public class MeetingChatMessage
  {
    public MeetingChatMessage(string message, DateTimeOffset date, int userId, int meetingId)
    {
      Message = message;
      Date = date;
      UserId = userId;
      MeetingId = meetingId;
    }

    public MeetingChatMessage(int id, string message, DateTimeOffset date, int userId, int meetingId)
      : this(message, date, userId, meetingId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public int UserId { get; private set; }
    public int MeetingId { get; private set; }

    public User User { get; private set; }
    public Meeting Meeting { get; private set; }
  }
}
