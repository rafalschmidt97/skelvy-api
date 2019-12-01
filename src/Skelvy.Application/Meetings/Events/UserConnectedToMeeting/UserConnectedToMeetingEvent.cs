using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserConnectedToMeeting
{
  public class UserConnectedToMeetingEvent : IEvent
  {
    public UserConnectedToMeetingEvent(int userId, int requestId, int meetingId, int groupId)
    {
      UserId = userId;
      RequestId = requestId;
      MeetingId = meetingId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int RequestId { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
  }
}
