using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Notifications;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Relations.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class SocketNotificationsService : ISocketNotificationsService
  {
    private readonly SignalRBackplane _socket;

    public SocketNotificationsService(SignalRBackplane socket)
    {
      _socket = socket;
    }

    public async Task BroadcastUserSentMessage(UserSentMessageAction action)
    {
      var data = new SocketNotificationData
      {
        Action = "UserSentMessage",
        RedirectTo = "chat",
        Data = action.Messages,
      };

      if (action.Message.Type == MessageType.Response)
      {
        if (action.Message.Text != null)
        {
          await SendNotification(
            action.UsersId,
            new SocketNotificationContent
            {
              Title = action.Message.UserName,
              Body = action.Message.Text,
            },
            NotificationType.Regular,
            data);
        }
        else
        {
          if (action.Message.AttachmentUrl != null)
          {
            await SendNotification(
              action.UsersId,
              new SocketNotificationContent
              {
                Title = action.Message.UserName,
                BodyLocKey = "USER_SENT_PHOTO",
              },
              NotificationType.Regular,
              data);
          }
          else
          {
            await SendNotification(
              action.UsersId,
              new SocketNotificationContent
              {
                Title = action.Message.UserName,
                BodyLocKey = "USER_SENT_MESSAGE",
              },
              NotificationType.Regular,
              data);
          }
        }
      }
      else
      {
        await SendNotification(
          action.UsersId,
          new SocketNotificationContent
          {
            Title = action.Message.UserName,
            BodyLocKey = "USER_SENT_MESSAGE",
          },
          NotificationType.NoPush,
          data);
      }
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_JOINED_MEETING",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserJoinedMeeting",
          RedirectTo = "meeting",
          Data = new { action.UserId },
        });
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_FOUND_MEETING",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserFoundMeeting",
          RedirectTo = "meeting",
        });
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_LEFT_MEETING",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserLeftMeeting",
          RedirectTo = "meeting",
          Data = action,
        });
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "MEETING_ABORTED",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "MeetingAborted",
          RedirectTo = "meeting",
          Data = action,
        });
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING_REQUEST",
          BodyLocKey = "MEETING_REQUEST_EXPIRED",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "MeetingRequestExpired",
          RedirectTo = "meeting",
        });
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "MEETING_EXPIRED",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "MeetingExpired",
          RedirectTo = "meeting",
        });
    }

    public async Task BroadcastUserRemoved(UserRemovedAction action)
    {
      await SendNotification(
        new List<int> { action.UserId },
        new SocketNotificationContent
        {
          TitleLocKey = "USER",
          BodyLocKey = "USER_REMOVED",
        },
        NotificationType.NoPush,
        new SocketNotificationData
        {
          Action = "UserRemoved",
        });
    }

    public async Task BroadcastUserDisabled(UserDisabledAction action)
    {
      await SendNotification(
        new List<int> { action.UserId },
        new SocketNotificationContent
        {
          TitleLocKey = "USER",
          BodyLocKey = "USER_DISABLED",
        },
        NotificationType.NoPush,
        new SocketNotificationData
        {
          Action = "UserDisabled",
        });
    }

    public async Task BroadcastUserSentFriendRequest(UserSentFriendRequestAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "FRIENDS",
          BodyLocKey = "NEW_FRIEND_REQUEST",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserSentFriendRequest",
          RedirectTo = "friends",
        });
    }

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestAction action)
    {
      await SendNotification(
        action.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "FRIENDS",
          BodyLocKey = action.IsAccepted ? "FRIEND_REQUEST_ACCEPTED" : "FRIEND_REQUEST_DENIED",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserRespondedFriendRequest",
          RedirectTo = "friends",
        });
    }

    private async Task SendNotification(
      IEnumerable<int> usersId,
      SocketNotificationContent notification,
      string type,
      SocketNotificationData data)
    {
      var message = new SocketNotificationMessage(notification, type, data);
      await _socket.PublishMessage(new SocketMessage(usersId, data.Action, message));
    }
  }
}
