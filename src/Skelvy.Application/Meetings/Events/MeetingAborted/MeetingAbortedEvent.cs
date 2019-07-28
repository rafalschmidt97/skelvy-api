using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MeetingAborted
{
  public class MeetingAbortedEvent : IEvent
  {
    public MeetingAbortedEvent(int userId, int meetingId, DateTimeOffset userLeftAt)
    {
      UserId = userId;
      MeetingId = meetingId;
      UserLeftAt = userLeftAt;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
    public DateTimeOffset UserLeftAt { get; private set; }
  }
}
