using System;

namespace Skelvy.Domain.Entities
{
  public class MeetingChatMessage
  {
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int MeetingId { get; set; }

    public User User { get; set; }
    public Meeting Meeting { get; set; }
  }
}
