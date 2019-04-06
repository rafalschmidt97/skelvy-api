using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IList<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserJoinedMeeting(MeetingUser user, IList<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserFoundMeeting(IList<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserLeftMeeting(MeetingUser user, IList<int> userIds, CancellationToken cancellationToken);
    Task BroadcastMeetingRequestExpired(IList<int> userIds, CancellationToken cancellationToken);
    Task BroadcastMeetingExpired(IList<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserCreated(User user, CancellationToken cancellationToken);
    Task BroadcastUserDeleted(User user, CancellationToken cancellationToken);
    Task BroadcastUserDisabled(User user, string reason, CancellationToken cancellationToken);
  }
}
