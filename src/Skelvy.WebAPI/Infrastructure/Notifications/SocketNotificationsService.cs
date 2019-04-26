using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Skelvy.Application.Notifications;
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
      await SendNotification("UserSentMeetingChatMessage", userIds, message);
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      await SendNotification("UserJoinedMeeting", userIds);
    }

    public async Task BroadcastUserFoundMeeting(IEnumerable<int> userIds)
    {
      await SendNotification("UserFoundMeeting", userIds);
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      await SendNotification("UserLeftMeeting", userIds);
    }

    public async Task BroadcastMeetingRequestExpired(IEnumerable<int> userIds)
    {
      await SendNotification("MeetingRequestExpired", userIds);
    }

    public async Task BroadcastMeetingExpired(IEnumerable<int> userIds)
    {
      await SendNotification("MeetingExpired", userIds);
    }

    private async Task SendNotification(string action, IEnumerable<int> userIds, object data)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync(action, data);
    }

    private async Task SendNotification(string action, IEnumerable<int> userIds)
    {
      await _hubContext.Clients.Users(PrepareUsers(userIds)).SendAsync(action);
    }

    private static IReadOnlyList<string> PrepareUsers(IEnumerable<int> userIds)
    {
      return userIds.Select(x => x.ToString()).ToList().AsReadOnly();
    }
  }
}
