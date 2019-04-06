namespace Skelvy.Domain.Entities
{
  public class MeetingUser
  {
    public int Id { get; set; }
    public string Status { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }

    public Meeting Meeting { get; set; }
    public User User { get; set; }
  }
}
