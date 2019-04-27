namespace Skelvy.Domain.Entities
{
  public class MeetingRequestDrink
  {
    public MeetingRequestDrink(int meetingRequestId, int drinkId)
    {
      MeetingRequestId = meetingRequestId;
      DrinkId = drinkId;
    }

    public MeetingRequestDrink(int meetingRequestId, int drinkId, MeetingRequest meetingRequest, Drink drink)
    {
      MeetingRequestId = meetingRequestId;
      DrinkId = drinkId;
      MeetingRequest = meetingRequest;
      Drink = drink;
    }

    public int MeetingRequestId { get; private set; }
    public int DrinkId { get; private set; }

    public MeetingRequest MeetingRequest { get; private set; }
    public Drink Drink { get; private set; }
  }
}
