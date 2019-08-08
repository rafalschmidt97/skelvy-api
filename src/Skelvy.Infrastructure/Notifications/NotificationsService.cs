using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
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

    public async Task BroadcastUserSentMessage(UserSentMessageAction action)
    {
      await _socketService.BroadcastUserSentMessage(action);

      if (action.Type == NotificationTypes.Regular)
      {
        await BroadcastActionToOffline(
          action.UsersId,
          async (offline) => await _pushService.BroadcastUserSentMessage(action, offline));
      }
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action)
    {
      await _socketService.BroadcastUserJoinedMeeting(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastUserJoinedMeeting(action, offline));
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action)
    {
      await _socketService.BroadcastUserFoundMeeting(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastUserFoundMeeting(action, offline));
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action)
    {
      await _socketService.BroadcastUserLeftMeeting(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastUserLeftMeeting(action, offline));
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedAction action)
    {
      await _socketService.BroadcastMeetingAborted(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastMeetingAborted(action, offline));
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action)
    {
      await _socketService.BroadcastMeetingRequestExpired(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastMeetingRequestExpired(action, offline));
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action)
    {
      await _socketService.BroadcastMeetingExpired(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastMeetingExpired(action, offline));
    }

    public async Task BroadcastUserCreated(UserCreatedAction action)
    {
      await _emailService.BroadcastUserCreated(action);
    }

    public async Task BroadcastUserRemoved(UserRemovedAction action)
    {
      await _socketService.BroadcastUserRemoved(action);
      await _emailService.BroadcastUserRemoved(action);
    }

    public async Task BroadcastUserDisabled(UserDisabledAction action)
    {
      await _socketService.BroadcastUserDisabled(action);
      await _emailService.BroadcastUserDisabled(action);
    }

    public async Task BroadcastUserSentFriendRequest(UserSentFriendRequestAction action)
    {
      await _socketService.BroadcastUserSentFriendRequest(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastUserSentFriendRequest(action, offline));
    }

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestAction action)
    {
      await _socketService.BroadcastUserRespondedFriendRequest(action);

      await BroadcastActionToOffline(
        action.UsersId,
        async (offline) => await _pushService.BroadcastUserRespondedFriendRequest(action, offline));
    }

    public static bool IsConnected(int userId)
    {
      return Connections.FirstOrDefault(x => x == userId) != default(int);
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
