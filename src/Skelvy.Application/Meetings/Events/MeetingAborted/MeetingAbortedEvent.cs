using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MeetingAborted
{
  public class MeetingAbortedEvent : IEvent
  {
    public MeetingAbortedEvent(int userId, int meetingId, int groupId, DateTimeOffset userLeftAt)
    {
      UserId = userId;
      MeetingId = meetingId;
      GroupId = groupId;
      UserLeftAt = userLeftAt;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public DateTimeOffset UserLeftAt { get; private set; }
  }
}
