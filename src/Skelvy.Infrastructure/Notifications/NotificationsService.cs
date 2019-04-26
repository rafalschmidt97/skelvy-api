using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skelvy.Application.Notifications;
using Skelvy.Domain.Entities;

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

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IList<int> usersId)
    {
      await BroadcastActionDependingOnConnection(
        usersId,
        async (online) => await _socketService.BroadcastUserSentMeetingChatMessage(message, online),
        async (offline) => await _pushService.BroadcastUserSentMeetingChatMessage(message, offline));
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IList<int> usersId)
    {
      await BroadcastActionDependingOnConnection(
        usersId,
        async (online) => await _socketService.BroadcastUserJoinedMeeting(user, online),
        async (offline) => await _pushService.BroadcastUserJoinedMeeting(user, offline));
    }

    public async Task BroadcastUserFoundMeeting(IList<int> usersId)
    {
      await BroadcastActionDependingOnConnection(
        usersId,
        async (online) => await _socketService.BroadcastUserFoundMeeting(online),
        async (offline) => await _pushService.BroadcastUserFoundMeeting(offline));
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IList<int> usersId)
    {
      await BroadcastActionDependingOnConnection(
        usersId,
        async (online) => await _socketService.BroadcastUserLeftMeeting(user, online),
        async (offline) => await _pushService.BroadcastUserLeftMeeting(user, offline));
    }

    public async Task BroadcastMeetingRequestExpired(IList<int> usersId)
    {
      await BroadcastActionDependingOnConnection(
        usersId,
        async (online) => await _socketService.BroadcastMeetingRequestExpired(online),
        async (offline) => await _pushService.BroadcastMeetingRequestExpired(offline));
    }

    public async Task BroadcastMeetingExpired(IList<int> usersId)
    {
      await BroadcastActionDependingOnConnection(
        usersId,
        async (online) => await _socketService.BroadcastMeetingExpired(online),
        async (offline) => await _pushService.BroadcastMeetingExpired(offline));
    }

    public async Task BroadcastUserCreated(User user)
    {
      await _emailService.BroadcastUserCreated(user);
    }

    public async Task BroadcastUserRemoved(User user)
    {
      await _emailService.BroadcastUserRemoved(user);
    }

    public async Task BroadcastUserDisabled(User user, string reason)
    {
      await _emailService.BroadcastUserDisabled(user, reason);
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
