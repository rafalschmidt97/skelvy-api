namespace Skelvy.Domain.Entities
{
  public class MeetingRequestDrinkType
  {
    public MeetingRequestDrinkType(int meetingRequestId, int drinkTypeId)
    {
      MeetingRequestId = meetingRequestId;
      DrinkTypeId = drinkTypeId;
    }

    public MeetingRequestDrinkType(int meetingRequestId, int drinkTypeId, MeetingRequest meetingRequest, DrinkType drinkType)
    {
      MeetingRequestId = meetingRequestId;
      DrinkTypeId = drinkTypeId;
      MeetingRequest = meetingRequest;
      DrinkType = drinkType;
    }

    public int MeetingRequestId { get; private set; }
    public int DrinkTypeId { get; private set; }

    public MeetingRequest MeetingRequest { get; private set; }
    public DrinkType DrinkType { get; private set; }
  }
}
