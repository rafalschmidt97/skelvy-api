using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;
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

    public async Task BroadcastUserSentMeetingChatMessage(UserSentMessageAction action, IEnumerable<int> usersId)
    {
      await SendNotification("UserSentMeetingChatMessage", usersId, new MeetingChatMessageDto
      {
        Message = action.Message,
        Date = action.Date,
        UserId = action.UserId,
        MeetingId = action.MeetingId,
      });
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification("UserJoinedMeeting", usersId, action);
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification("UserFoundMeeting", usersId);
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification("UserLeftMeeting", usersId, action);
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action, IEnumerable<int> usersId)
    {
      await SendNotification("MeetingRequestExpired", usersId);
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action, IEnumerable<int> usersId)
    {
      await SendNotification("MeetingExpired", usersId);
    }

    public async Task BroadcastUserRemoved(UserRemovedAction action, IEnumerable<int> usersId)
    {
      await SendNotification("UserRemoved", usersId);
    }

    public async Task BroadcastUserDisabled(UserDisabledAction action, IEnumerable<int> usersId)
    {
      await SendNotification("UserDisabled", usersId);
    }

    private async Task SendNotification(string action, IEnumerable<int> usersId, object data)
    {
      await _hubContext.Clients.Users(PrepareUsers(usersId)).SendAsync(action, data);
    }

    private async Task SendNotification(string action, int userId, object data)
    {
      await _hubContext.Clients.User(PrepareUser(userId)).SendAsync(action, data);
    }

    private async Task SendNotification(string action, IEnumerable<int> usersId)
    {
      await _hubContext.Clients.Users(PrepareUsers(usersId)).SendAsync(action);
    }

    private async Task SendNotification(string action, int userId)
    {
      await _hubContext.Clients.User(PrepareUser(userId)).SendAsync(action);
    }

    private static IReadOnlyList<string> PrepareUsers(IEnumerable<int> usersId)
    {
      return usersId.Select(PrepareUser).ToList().AsReadOnly();
    }

    private static string PrepareUser(int userId)
    {
      return userId.ToString();
    }
  }
}
