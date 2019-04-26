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

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> usersId)
    {
      await SendNotification("UserSentMeetingChatMessage", usersId, message);
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> usersId)
    {
      await SendNotification("UserJoinedMeeting", usersId);
    }

    public async Task BroadcastUserFoundMeeting(IEnumerable<int> usersId)
    {
      await SendNotification("UserFoundMeeting", usersId);
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> usersId)
    {
      await SendNotification("UserLeftMeeting", usersId);
    }

    public async Task BroadcastMeetingRequestExpired(IEnumerable<int> usersId)
    {
      await SendNotification("MeetingRequestExpired", usersId);
    }

    public async Task BroadcastMeetingExpired(IEnumerable<int> usersId)
    {
      await SendNotification("MeetingExpired", usersId);
    }

    private async Task SendNotification(string action, IEnumerable<int> usersId, object data)
    {
      await _hubContext.Clients.Users(PrepareUsers(usersId)).SendAsync(action, data);
    }

    private async Task SendNotification(string action, IEnumerable<int> usersId)
    {
      await _hubContext.Clients.Users(PrepareUsers(usersId)).SendAsync(action);
    }

    private static IReadOnlyList<string> PrepareUsers(IEnumerable<int> usersId)
    {
      return usersId.Select(x => x.ToString()).ToList().AsReadOnly();
    }
  }
}
