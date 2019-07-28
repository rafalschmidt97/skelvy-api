using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserConnectedToMeeting
{
  public class UserConnectedToMeetingEvent : IEvent
  {
    public UserConnectedToMeetingEvent(int userId, int meetingId)
    {
      UserId = userId;
      MeetingId = meetingId;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
