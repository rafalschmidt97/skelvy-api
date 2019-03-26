using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, ICollection<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserJoinedMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserFoundMeeting(ICollection<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserLeftMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken);
    Task BroadcastMeetingRequestExpired(ICollection<int> userIds, CancellationToken cancellationToken);
    Task BroadcastMeetingExpired(ICollection<int> userIds, CancellationToken cancellationToken);
    Task BroadcastUserCreated(User user, CancellationToken cancellationToken);
    Task BroadcastUserDeleted(User user, CancellationToken cancellationToken);
  }
}
