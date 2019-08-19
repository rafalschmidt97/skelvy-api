using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserJoinedMeeting
{
  public class UserJoinedMeetingEvent : IEvent
  {
    public UserJoinedMeetingEvent(int userId, int groupId)
    {
      UserId = userId;
      GroupId = groupId;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
  }
}
