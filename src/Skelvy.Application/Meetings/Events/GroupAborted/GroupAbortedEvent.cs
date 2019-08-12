using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.GroupAborted
{
  public class GroupAbortedEvent : IEvent
  {
    public GroupAbortedEvent(int userId, int groupId, DateTimeOffset userLeftAt)
    {
      UserId = userId;
      GroupId = groupId;
      UserLeftAt = userLeftAt;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
    public DateTimeOffset UserLeftAt { get; private set; }
  }
}
