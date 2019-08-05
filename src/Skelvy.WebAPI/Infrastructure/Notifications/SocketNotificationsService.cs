using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Queries;
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
            SocketNotificationTypes.Regular,
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
              SocketNotificationTypes.Regular,
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
              SocketNotificationTypes.Regular,
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
          SocketNotificationTypes.NoPush,
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
        SocketNotificationTypes.Regular,
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
        SocketNotificationTypes.Regular,
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
        SocketNotificationTypes.Regular,
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
        SocketNotificationTypes.Regular,
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
        SocketNotificationTypes.Regular,
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
        SocketNotificationTypes.Regular,
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
        SocketNotificationTypes.NoPush,
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
        SocketNotificationTypes.NoPush,
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
