namespace Skelvy.Domain.Entities
{
  public class MeetingRequestDrinkType
  {
    public MeetingRequestDrinkType(int meetingRequestId, int drinkTypeId)
    {
      MeetingRequestId = meetingRequestId;
      DrinkTypeId = drinkTypeId;
    }

    public int MeetingRequestId { get; set; }
    public int DrinkTypeId { get; set; }

    public MeetingRequest MeetingRequest { get; set; }
    public DrinkType DrinkType { get; set; }
  }
}
