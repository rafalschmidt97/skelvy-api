using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserLeftMeeting
{
  public class UserLeftMeetingEvent : IEvent
  {
    public UserLeftMeetingEvent(int userId, int meetingId)
    {
      UserId = userId;
      MeetingId = meetingId;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
