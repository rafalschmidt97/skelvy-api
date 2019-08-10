namespace Skelvy.Domain.Entities
{
  public class MeetingRequestActivity
  {
    public MeetingRequestActivity(int meetingRequestId, int activityId)
    {
      MeetingRequestId = meetingRequestId;
      ActivityId = activityId;
    }

    public int MeetingRequestId { get; set; }
    public int ActivityId { get; set; }

    public MeetingRequest MeetingRequest { get; set; }
    public Activity Activity { get; set; }
  }
}
