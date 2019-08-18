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
    public static readonly List<Connection> Connections = new List<Connection>();
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
          async offline => await _pushService.BroadcastUserSentMessage(notification, offline));
      }
    }

    public async Task BroadcastUserJoinedGroup(UserJoinedGroupNotification notification)
    {
      await _socketService.BroadcastUserJoinedGroup(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserJoinedGroup(notification, offline));
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification)
    {
      await _socketService.BroadcastUserFoundMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserFoundMeeting(notification, offline));
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification)
    {
      await _socketService.BroadcastUserLeftMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserLeftMeeting(notification, offline));
    }

    public async Task BroadcastUserRemovedFromMeeting(UserRemovedFromMeetingNotification notification)
    {
      await _socketService.BroadcastUserRemovedFromMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserRemovedFromMeeting(notification, offline));
    }

    public async Task BroadcastUserLeftGroup(UserLeftGroupNotification notification)
    {
      await _socketService.BroadcastUserLeftGroup(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserLeftGroup(notification, offline));
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedNotification notification)
    {
      await _socketService.BroadcastMeetingAborted(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async (offline) => await _pushService.BroadcastMeetingAborted(notification, offline));
    }

    public async Task BroadcastGroupAborted(GroupAbortedNotification notification)
    {
      await _socketService.BroadcastGroupAborted(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastGroupAborted(notification, offline));
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification)
    {
      await _socketService.BroadcastMeetingRequestExpired(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastMeetingRequestExpired(notification, offline));
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredNotification notification)
    {
      await _socketService.BroadcastMeetingExpired(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastMeetingExpired(notification, offline));
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
        async offline => await _pushService.BroadcastUserSentFriendRequest(notification, offline));
    }

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestNotification notification)
    {
      await _socketService.BroadcastUserRespondedFriendRequest(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserRespondedFriendRequest(notification, offline));
    }

    public static bool IsConnected(int userId)
    {
      return Connections.Any(x => x.UserId == userId);
    }

    private static Task BroadcastActionToOffline(
      IEnumerable<int> usersId,
      Action<IList<int>> pushNotification)
    {
      var offlineUsersId = usersId.Where(userId => !IsConnected(userId)).ToList();

      if (offlineUsersId.Any())
      {
        pushNotification(offlineUsersId);
      }

      return Task.CompletedTask;
    }
  }

  public class Connection
  {
    public Connection(int userId, string connectionId)
    {
      UserId = userId;
      ConnectionId = connectionId;
    }

    public int UserId { get; }
    public string ConnectionId { get; }
  }
}
