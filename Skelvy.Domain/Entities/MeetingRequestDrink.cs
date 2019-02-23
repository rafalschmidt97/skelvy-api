namespace Skelvy.Domain.Entities
{
  public class MeetingRequestDrink
  {
    public int MeetingRequestId { get; set; }
    public int DrinkId { get; set; }

    public MeetingRequest MeetingRequest { get; set; }
    public Drink Drink { get; set; }
  }
}
