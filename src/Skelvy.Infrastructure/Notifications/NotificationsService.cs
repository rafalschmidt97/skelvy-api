using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Domain.Entities;

namespace Skelvy.Infrastructure.Notifications
{
  public class NotificationsService : INotificationsService
  {
    public static readonly HashSet<int> Connections = new HashSet<int>();
    private readonly IPushNotificationsService _pushService;
    private readonly ISocketNotificationsService _socketService;

    public NotificationsService(IPushNotificationsService pushService, ISocketNotificationsService socketService)
    {
      _pushService = pushService;
      _socketService = socketService;
    }

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var connections = GetConnections(userIds);

      if (connections.OnlineIds.Count > 0)
      {
        await _socketService.BroadcastUserSentMeetingChatMessage(message, userIds, cancellationToken);
      }

      if (connections.OfflineIds.Count > 0)
      {
        await _pushService.BroadcastUserSentMeetingChatMessage(message, userIds, cancellationToken);
      }
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var connections = GetConnections(userIds);

      if (connections.OnlineIds.Count > 0)
      {
        await _socketService.BroadcastUserJoinedMeeting(user, userIds, cancellationToken);
      }

      if (connections.OfflineIds.Count > 0)
      {
        await _pushService.BroadcastUserJoinedMeeting(user, userIds, cancellationToken);
      }
    }

    public async Task BroadcastUserFoundMeeting(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var connections = GetConnections(userIds);

      if (connections.OnlineIds.Count > 0)
      {
        await _socketService.BroadcastUserFoundMeeting(userIds, cancellationToken);
      }

      if (connections.OfflineIds.Count > 0)
      {
        await _pushService.BroadcastUserFoundMeeting(userIds, cancellationToken);
      }
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var connections = GetConnections(userIds);

      if (connections.OnlineIds.Count > 0)
      {
        await _socketService.BroadcastUserLeftMeeting(user, userIds, cancellationToken);
      }

      if (connections.OfflineIds.Count > 0)
      {
        await _pushService.BroadcastUserLeftMeeting(user, userIds, cancellationToken);
      }
    }

    public async Task BroadcastMeetingRequestExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var connections = GetConnections(userIds);

      if (connections.OnlineIds.Count > 0)
      {
        await _socketService.BroadcastMeetingRequestExpired(userIds, cancellationToken);
      }

      if (connections.OfflineIds.Count > 0)
      {
        await _pushService.BroadcastMeetingRequestExpired(userIds, cancellationToken);
      }
    }

    public async Task BroadcastMeetingExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var connections = GetConnections(userIds);

      if (connections.OnlineIds.Count > 0)
      {
        await _socketService.BroadcastMeetingExpired(userIds, cancellationToken);
      }

      if (connections.OfflineIds.Count > 0)
      {
        await _pushService.BroadcastMeetingExpired(userIds, cancellationToken);
      }
    }

    public static bool IsConnected(int userId)
    {
      return Connections.FirstOrDefault(x => x == userId) != default(int);
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

    public ICollection<int> OnlineIds { get; private set; }
    public ICollection<int> OfflineIds { get; private set; }
  }
}
