using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MeetingRequestsExpired
{
  public class MeetingRequestsExpiredEvent : IEvent
  {
    public MeetingRequestsExpiredEvent(IEnumerable<int> requestsId)
    {
      RequestsId = requestsId;
    }

    public IEnumerable<int> RequestsId { get; private set; }
  }
}
