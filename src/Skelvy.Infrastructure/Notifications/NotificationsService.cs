using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Notifications;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Relations.Infrastructure.Notifications;
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

    public async Task BroadcastUserSentMessage(UserSentMessageNotification notification)
    {
      await _socketService.BroadcastUserSentMessage(notification);

      if (notification.Type == NotificationType.Regular)
      {
        await BroadcastActionToOffline(
          notification.UsersId,
          async (offline) => await _pushService.BroadcastUserSentMessage(notification, offline));
      }
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification)
    {
      await _socketService.BroadcastUserJoinedMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastUserJoinedMeeting(notification, offline));
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification)
    {
      await _socketService.BroadcastUserFoundMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastUserFoundMeeting(notification, offline));
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification)
    {
      await _socketService.BroadcastUserLeftMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastUserLeftMeeting(notification, offline));
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedNotification notification)
    {
      await _socketService.BroadcastMeetingAborted(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastMeetingAborted(notification, offline));
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification)
    {
      await _socketService.BroadcastMeetingRequestExpired(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastMeetingRequestExpired(notification, offline));
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredNotification notification)
    {
      await _socketService.BroadcastMeetingExpired(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastMeetingExpired(notification, offline));
    }

    public async Task BroadcastUserCreated(UserCreatedNotification notification)
    {
      await _emailService.BroadcastUserCreated(notification);
    }

    public async Task BroadcastUserRemoved(UserRemovedNotification notification)
    {
      await _socketService.BroadcastUserRemoved(notification);
      await _emailService.BroadcastUserRemoved(notification);
    }

    public async Task BroadcastUserDisabled(UserDisabledNotification notification)
    {
      await _socketService.BroadcastUserDisabled(notification);
      await _emailService.BroadcastUserDisabled(notification);
    }

    public async Task BroadcastUserSentFriendRequest(UserSentFriendRequestNotification notification)
    {
      await _socketService.BroadcastUserSentFriendRequest(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastUserSentFriendRequest(notification, offline));
    }

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestNotification notification)
    {
      await _socketService.BroadcastUserRespondedFriendRequest(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastUserRespondedFriendRequest(notification, offline));
    }

    public static bool IsConnected(int userId)
    {
      return Connections.FirstOrDefault(x => x == userId) != default;
    }

    private static Task BroadcastActionToOffline(
      IEnumerable<int> connected,
      Action<IList<int>> pushNotification)
    {
      var connections = GetConnections(connected);

      if (connections.Offline.Any())
      {
        pushNotification(connections.Offline);
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
