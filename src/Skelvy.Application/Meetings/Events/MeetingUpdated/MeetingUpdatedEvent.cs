using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MeetingUpdated
{
  public class MeetingUpdatedEvent : IEvent
  {
    public MeetingUpdatedEvent(int userId, int meetingId, int groupId)
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
