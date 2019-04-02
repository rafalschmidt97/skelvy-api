using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Notifications
{
  public interface IPushNotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserFoundMeeting(IEnumerable<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> userIds, CancellationToken cancellationToken);
    Task BroadcastMeetingRequestExpired(IEnumerable<int> userIds, CancellationToken cancellationToken);
    Task BroadcastMeetingExpired(IEnumerable<int> userIds, CancellationToken cancellationToken);
  }
}
