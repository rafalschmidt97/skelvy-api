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

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IList<int> userIds)
    {
      await BroadcastActionDependingOnConnection(
        userIds,
        async (onlineIds) => await _socketService.BroadcastUserSentMeetingChatMessage(message, onlineIds),
        async (offlineIds) => await _pushService.BroadcastUserSentMeetingChatMessage(message, offlineIds));
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IList<int> userIds)
    {
      await BroadcastActionDependingOnConnection(
        userIds,
        async (onlineIds) => await _socketService.BroadcastUserJoinedMeeting(user, onlineIds),
        async (offlineIds) => await _pushService.BroadcastUserJoinedMeeting(user, offlineIds));
    }

    public async Task BroadcastUserFoundMeeting(IList<int> userIds)
    {
      await BroadcastActionDependingOnConnection(
        userIds,
        async (onlineIds) => await _socketService.BroadcastUserFoundMeeting(onlineIds),
        async (offlineIds) => await _pushService.BroadcastUserFoundMeeting(offlineIds));
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IList<int> userIds)
    {
      await BroadcastActionDependingOnConnection(
        userIds,
        async (onlineIds) => await _socketService.BroadcastUserLeftMeeting(user, onlineIds),
        async (offlineIds) => await _pushService.BroadcastUserLeftMeeting(user, offlineIds));
    }

    public async Task BroadcastMeetingRequestExpired(IList<int> userIds)
    {
      await BroadcastActionDependingOnConnection(
        userIds,
        async (onlineIds) => await _socketService.BroadcastMeetingRequestExpired(onlineIds),
        async (offlineIds) => await _pushService.BroadcastMeetingRequestExpired(offlineIds));
    }

    public async Task BroadcastMeetingExpired(IList<int> userIds)
    {
      await BroadcastActionDependingOnConnection(
        userIds,
        async (onlineIds) => await _socketService.BroadcastMeetingExpired(onlineIds),
        async (offlineIds) => await _pushService.BroadcastMeetingExpired(offlineIds));
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
      IEnumerable<int> connectedIds,
      Action<IList<int>> socketAction,
      Action<IList<int>> pushAction)
    {
      var connections = GetConnections(connectedIds);

      if (connections.OnlineIds.Count > 0)
      {
        socketAction(connections.OnlineIds);
      }

      if (connections.OfflineIds.Count > 0)
      {
        pushAction(connections.OfflineIds);
      }

      return Task.CompletedTask;
    }

    private static Connections GetConnections(IEnumerable<int> userIds)
    {
      var connections = new Connections();

      foreach (var userId in userIds)
      {
        if (IsConnected(userId))
        {
          connections.OnlineIds.Add(userId);
        }
        else
        {
          connections.OfflineIds.Add(userId);
        }
      }

      return connections;
    }
  }

  public class Connections
  {
    public Connections()
    {
      OnlineIds = new List<int>();
      OfflineIds = new List<int>();
    }

    public IList<int> OnlineIds { get; }
    public IList<int> OfflineIds { get; }
  }
}
