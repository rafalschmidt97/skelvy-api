using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserJoinedMeeting
{
  public class UserJoinedMeetingEvent : IEvent
  {
    public UserJoinedMeetingEvent(int userId, int meetingId)
    {
      UserId = userId;
      MeetingId = meetingId;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
