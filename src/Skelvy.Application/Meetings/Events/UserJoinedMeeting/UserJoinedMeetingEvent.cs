using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserJoinedMeeting
{
  public class UserJoinedMeetingEvent : IEvent
  {
    public UserJoinedMeetingEvent(int userId, string userRole, int meetingId, int groupId)
    {
      UserId = userId;
      UserRole = userRole;
      MeetingId = meetingId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public string UserRole { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
  }
}
