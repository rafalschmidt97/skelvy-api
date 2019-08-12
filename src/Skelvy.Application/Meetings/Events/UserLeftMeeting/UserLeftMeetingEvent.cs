using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserLeftMeeting
{
  public class UserLeftMeetingEvent : IEvent
  {
    public UserLeftMeetingEvent(int userId, int groupId)
    {
      UserId = userId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
