using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserLeftMeeting
{
  public class UserLeftMeetingEvent : IEvent
  {
    public UserLeftMeetingEvent(int userId, int groupId, int meetingId)
    {
      UserId = userId;
      GroupId = groupId;
      MeetingId = meetingId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
