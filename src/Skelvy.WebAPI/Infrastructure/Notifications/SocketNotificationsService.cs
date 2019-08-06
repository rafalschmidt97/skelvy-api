using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;
using Skelvy.Application.Notifications.Infrastructure;
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

      if (action.Message.Type == MessageTypes.Response)
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
            NotificationTypes.Regular,
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
              NotificationTypes.Regular,
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
              NotificationTypes.Regular,
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
          NotificationTypes.NoPush,
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
        NotificationTypes.Regular,
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
        NotificationTypes.Regular,
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
        NotificationTypes.Regular,
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
        NotificationTypes.Regular,
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
        NotificationTypes.Regular,
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
        NotificationTypes.Regular,
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
        NotificationTypes.NoPush,
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
        NotificationTypes.NoPush,
        new SocketNotificationData
        {
          Action = "UserDisabled",
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
