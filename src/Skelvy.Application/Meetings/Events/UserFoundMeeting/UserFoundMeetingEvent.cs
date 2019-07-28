using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserFoundMeeting
{
  public class UserFoundMeetingEvent : IEvent
  {
    public UserFoundMeetingEvent(int userId, int meetingId)
    {
      UserId = userId;
      MeetingId = meetingId;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
