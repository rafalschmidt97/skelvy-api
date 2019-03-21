using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.WebAPI.Hubs;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class SocketNotificationsService : ISocketNotificationsService
  {
    private readonly IHubContext<UsersHub> _hubContext;

    public SocketNotificationsService(IHubContext<UsersHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserSentMeetingChatMessage", message, cancellationToken);
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserJoinedMeeting", cancellationToken);
    }

    public async Task BroadcastUserFoundMeeting(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserFoundMeeting", cancellationToken);
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserLeftMeeting", cancellationToken);
    }

    public async Task BroadcastMeetingRequestExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("MeetingRequestExpired", cancellationToken);
    }

    public async Task BroadcastMeetingExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("MeetingExpired", cancellationToken);
    }

    private static IReadOnlyList<string> PrepareUsers(IEnumerable<int> userIds)
    {
      return userIds.Select(x => x.ToString()).ToList().AsReadOnly();
    }
  }
}
