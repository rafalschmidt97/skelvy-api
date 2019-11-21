using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserConnectedToMeeting
{
  public class UserConnectedToMeetingEvent : IEvent
  {
    public UserConnectedToMeetingEvent(int userId, int meetingId, int groupId)
    {
      UserId = userId;
      MeetingId = meetingId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
  }
}
