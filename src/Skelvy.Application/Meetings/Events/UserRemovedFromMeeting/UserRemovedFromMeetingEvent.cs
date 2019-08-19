using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UserRemovedFromMeeting
{
  public class UserRemovedFromMeetingEvent : IEvent
  {
    public UserRemovedFromMeetingEvent(int userId, int removedUserId, int groupId, int meetingId)
    {
      UserId = userId;
      RemovedUserId = removedUserId;
      GroupId = groupId;
      MeetingId = meetingId;
    }

    public int UserId { get; private set; }
    public int RemovedUserId { get; private set; }
    public int GroupId { get; private set; }
    public int MeetingId { get; private set; }
  }
}
