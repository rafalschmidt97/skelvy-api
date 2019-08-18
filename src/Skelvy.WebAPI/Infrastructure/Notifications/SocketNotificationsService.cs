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

    public async Task BroadcastUserSentMessage(UserSentMessageNotification notification)
    {
      var data = new SocketNotificationData
      {
        Action = "UserSentMessage",
        RedirectTo = "chat",
        Data = notification.Messages,
      };

      if (notification.Message.Type == MessageType.Response)
      {
        if (notification.Message.Text != null)
        {
          await SendNotification(
            notification.UsersId,
            new SocketNotificationContent
            {
              Title = notification.Message.UserName,
              Body = notification.Message.Text,
            },
            NotificationType.Regular,
            data);
        }
        else
        {
          if (notification.Message.AttachmentUrl != null)
          {
            await SendNotification(
              notification.UsersId,
              new SocketNotificationContent
              {
                Title = notification.Message.UserName,
                BodyLocKey = "USER_SENT_PHOTO",
              },
              NotificationType.Regular,
              data);
          }
          else
          {
            await SendNotification(
              notification.UsersId,
              new SocketNotificationContent
              {
                Title = notification.Message.UserName,
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
          notification.UsersId,
          new SocketNotificationContent
          {
            Title = notification.Message.UserName,
            BodyLocKey = "USER_SENT_MESSAGE",
          },
          NotificationType.NoPush,
          data);
      }
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastUserRemovedFromMeeting(UserRemovedFromMeetingNotification notification)
    {
      await SendNotification(
        notification.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_REMOVED_FROM_MEETING",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserRemovedFromMeeting",
          RedirectTo = "meeting",
          Data = new { notification.UserId, notification.RemovedUserId },
        });
    }

    public async Task BroadcastUserLeftGroup(UserLeftGroupNotification notification)
    {
      await SendNotification(
        notification.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "GROUP",
          BodyLocKey = "USER_LEFT_GROUP",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "UserLeftGroup",
          RedirectTo = "group",
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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
        });
    }

    public async Task BroadcastGroupAborted(GroupAbortedNotification notification)
    {
      await SendNotification(
        notification.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "GROUP",
          BodyLocKey = "GROUP_ABORTED",
        },
        NotificationType.Regular,
        new SocketNotificationData
        {
          Action = "GroupAborted",
          RedirectTo = "group",
        });
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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

    public async Task BroadcastMeetingExpired(MeetingExpiredNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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

    public async Task BroadcastUserRemoved(UserRemovedNotification notification)
    {
      await SendNotification(
        new List<int> { notification.UserId },
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

    public async Task BroadcastUserDisabled(UserDisabledNotification notification)
    {
      await SendNotification(
        new List<int> { notification.UserId },
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

    public async Task BroadcastUserSentFriendRequest(UserSentFriendRequestNotification notification)
    {
      await SendNotification(
        notification.UsersId,
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

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestNotification notification)
    {
      await SendNotification(
        notification.UsersId,
        new SocketNotificationContent
        {
          TitleLocKey = "FRIENDS",
          BodyLocKey = notification.IsAccepted ? "FRIEND_REQUEST_ACCEPTED" : "FRIEND_REQUEST_DENIED",
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
