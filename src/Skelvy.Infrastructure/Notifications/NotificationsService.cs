using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skelvy.Application.Groups.Infrastructure.Notifications;
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

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification)
    {
      await _socketService.BroadcastUserJoinedMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserJoinedMeeting(notification, offline));
    }

    public async Task BroadcastUserConnectedToMeeting(UserConnectedToMeetingNotification notification)
    {
      await _socketService.BroadcastUserConnectedToMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserConnectedToMeeting(notification, offline));
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

    public async Task BroadcastUserSelfRemovedFromMeeting(UserRemovedFromMeetingNotification notification)
    {
      await _socketService.BroadcastUserSelfRemovedFromMeeting(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserSelfRemovedFromMeeting(notification, offline));
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
        async offline => await _pushService.BroadcastMeetingAborted(notification, offline));
    }

    public async Task BroadcastMeetingUpdated(MeetingUpdatedNotification notification)
    {
      await _socketService.BroadcastMeetingUpdated(notification);
    }

    public async Task BroadcastGroupUpdated(GroupUpdatedNotification notification)
    {
      await _socketService.BroadcastGroupUpdated(notification);
    }

    public async Task BroadcastMeetingUserRoleUpdated(MeetingUserRoleUpdatedNotification notification)
    {
      await _socketService.BroadcastMeetingUserRoleUpdated(notification);
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

    public async Task BroadcastUserSentFriendInvitation(UserSentFriendInvitationNotification notification)
    {
      await _socketService.BroadcastUserSentFriendInvitation(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserSentFriendInvitation(notification, offline));
    }

    public async Task BroadcastUserRespondedFriendInvitation(UserRespondedFriendInvitationNotification notification)
    {
      await _socketService.BroadcastUserRespondedFriendInvitation(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserRespondedFriendInvitation(notification, offline));
    }

    public async Task BroadcastFriendRemoved(FriendRemovedNotification notification)
    {
      await _socketService.BroadcastFriendRemoved(notification);
    }

    public async Task BroadcastUserSentMeetingInvitation(UserSentMeetingInvitationNotification notification)
    {
      await _socketService.BroadcastUserSentMeetingInvitation(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserSentMeetingInvitation(notification, offline));
    }

    public async Task BroadcastUserRespondedMeetingInvitation(UserRespondedMeetingInvitationNotification notification)
    {
      await _socketService.BroadcastUserRespondedMeetingInvitation(notification);

      await BroadcastActionToOffline(
        notification.UsersId,
        async offline => await _pushService.BroadcastUserRespondedMeetingInvitation(notification, offline));
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
