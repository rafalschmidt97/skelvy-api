using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Skelvy.Application.Infrastructure.Notifications;
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

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserSentMeetingChatMessage", message);
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserJoinedMeeting");
    }

    public async Task BroadcastUserFoundMeeting(IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserFoundMeeting");
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("UserLeftMeeting");
    }

    public async Task BroadcastMeetingRequestExpired(IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("MeetingRequestExpired");
    }

    public async Task BroadcastMeetingExpired(IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync("MeetingExpired");
    }

    private static IReadOnlyList<string> PrepareUsers(IEnumerable<int> userIds)
    {
      return userIds.Select(x => x.ToString()).ToList().AsReadOnly();
    }
  }
}
