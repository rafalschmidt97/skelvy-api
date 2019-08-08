using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MeetingsExpired
{
  public class MeetingsExpiredEvent : IEvent
  {
    public MeetingsExpiredEvent(IEnumerable<int> meetingsId)
    {
      MeetingsId = meetingsId;
    }

    public IEnumerable<int> MeetingsId { get; private set; }
  }
}
