using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Infrastructure.Notifications
{
  public class NotificationsService : INotificationsService
  {
    public static readonly HashSet<int> Connections = new HashSet<int>();
    private readonly IPushNotificationsService _pushService;
    private readonly ISocketNotificationsService _socketService;
    private readonly IEmailNotificationsService _emailService;

    public NotificationsService(
      IPushNotificationsService pushService,
      ISocketNotificationsService socketService,
      IEmailNotificationsService emailService)
    {
      _pushService = pushService;
      _socketService = socketService;
      _emailService = emailService;
    }

    public async Task BroadcastUserSentMessage(UserSentMessageAction action)
    {
      await BroadcastActionDependingOnConnection(
        action.UsersId,
        async (online) => await _socketService.BroadcastUserSentMeetingChatMessage(action, online),
        async (offline) => await _pushService.BroadcastUserSentMeetingChatMessage(action, offline));
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action)
    {
      await BroadcastActionDependingOnConnection(
        action.UsersId,
        async (online) => await _socketService.BroadcastUserJoinedMeeting(action, online),
        async (offline) => await _pushService.BroadcastUserJoinedMeeting(action, offline));
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action)
    {
      await BroadcastActionDependingOnConnection(
        action.UsersId,
        async (online) => await _socketService.BroadcastUserFoundMeeting(action, online),
        async (offline) => await _pushService.BroadcastUserFoundMeeting(action, offline));
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action)
    {
      await BroadcastActionDependingOnConnection(
        action.UsersId,
        async (online) => await _socketService.BroadcastUserLeftMeeting(action, online),
        async (offline) => await _pushService.BroadcastUserLeftMeeting(action, offline));
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action)
    {
      await BroadcastActionDependingOnConnection(
        action.UsersId,
        async (online) => await _socketService.BroadcastMeetingRequestExpired(action, online),
        async (offline) => await _pushService.BroadcastMeetingRequestExpired(action, offline));
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action)
    {
      await BroadcastActionDependingOnConnection(
        action.UsersId,
        async (online) => await _socketService.BroadcastMeetingExpired(action, online),
        async (offline) => await _pushService.BroadcastMeetingExpired(action, offline));
    }

    public async Task BroadcastUserCreated(UserCreatedAction action)
    {
      await _emailService.BroadcastUserCreated(action);
    }

    public async Task BroadcastUserRemoved(UserRemovedAction action)
    {
      await BroadcastActionToOnline(
        new List<int> { action.UserId },
        async (online) => await _socketService.BroadcastUserRemoved(action, online));

      await _emailService.BroadcastUserRemoved(action);
    }

    public async Task BroadcastUserDisabled(UserDisabledAction action)
    {
      await BroadcastActionToOnline(
        new List<int> { action.UserId },
        async (online) => await _socketService.BroadcastUserDisabled(action, online));

      await _emailService.BroadcastUserDisabled(action);
    }

    public static bool IsConnected(int userId)
    {
      return Connections.FirstOrDefault(x => x == userId) != default(int);
    }

    private static Task BroadcastActionDependingOnConnection(
      IEnumerable<int> connected,
      Action<IList<int>> socketAction,
      Action<IList<int>> pushAction)
    {
      var connections = GetConnections(connected);

      if (connections.Online.Count > 0)
      {
        socketAction(connections.Online);
      }

      if (connections.Offline.Count > 0)
      {
        pushAction(connections.Offline);
      }

      return Task.CompletedTask;
    }

    private static Task BroadcastActionToOnline(
      IEnumerable<int> connected,
      Action<IList<int>> socketAction)
    {
      var connections = GetConnections(connected);

      if (connections.Online.Count > 0)
      {
        socketAction(connections.Online);
      }

      return Task.CompletedTask;
    }

    private static Task BroadcastActionToOffline(
      IEnumerable<int> connected,
      Action<IList<int>> pushAction)
    {
      var connections = GetConnections(connected);

      if (connections.Offline.Count > 0)
      {
        pushAction(connections.Offline);
      }

      return Task.CompletedTask;
    }

    private static Connections GetConnections(IEnumerable<int> usersId)
    {
      var connections = new Connections();

      foreach (var userId in usersId)
      {
        if (IsConnected(userId))
        {
          connections.Online.Add(userId);
        }
        else
        {
          connections.Offline.Add(userId);
        }
      }

      return connections;
    }
  }

  public class Connections
  {
    public Connections()
    {
      Online = new List<int>();
      Offline = new List<int>();
    }

    public IList<int> Online { get; }
    public IList<int> Offline { get; }
  }
}
