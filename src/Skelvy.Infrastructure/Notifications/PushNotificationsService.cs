using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Infrastructure.Notifications
{
  public class PushNotificationsService : HttpServiceBase, IPushNotificationsService
  {
    public PushNotificationsService(IConfiguration configuration)
      : base("https://fcm.googleapis.com/fcm/")
    {
      HttpClient.DefaultRequestHeaders
        .TryAddWithoutValidation("Authorization", "key=" + configuration["SKELVY_GOOGLE_KEY_WEB"]);
    }

    public async Task BroadcastUserSentMessage(UserSentMessageAction action, IEnumerable<int> usersId)
    {
      var data = new PushNotificationData
      {
        Action = "UserSentMessage",
        RedirectTo = "chat",
        Data = action.Messages,
      };

      if (action.Message.Text != null)
      {
        await SendNotification(
          usersId,
          new PushNotificationContent
          {
            Title = action.Message.UserName,
            Body = action.Message.Text,
          },
          data);
      }
      else
      {
        if (action.Message.AttachmentUrl != null)
        {
          await SendNotification(
            usersId,
            new PushNotificationContent
            {
              Title = action.Message.UserName,
              BodyLocKey = "USER_SENT_PHOTO",
            },
            data);
        }
        else
        {
          await SendNotification(
            usersId,
            new PushNotificationContent
            {
              Title = action.Message.UserName,
              BodyLocKey = "USER_SENT_MESSAGE",
            },
            data);
        }
      }
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_JOINED_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserJoinedMeeting",
          RedirectTo = "meeting",
          Data = new { action.UserId },
        });
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_FOUND_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserFoundMeeting",
          RedirectTo = "meeting",
        });
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "USER_LEFT_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserLeftMeeting",
          RedirectTo = "meeting",
          Data = new { action.UserId },
        });
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "MEETING_ABORTED",
        },
        new PushNotificationData
        {
          Action = "MeetingAborted",
          RedirectTo = "meeting",
          Data = new { action.UserId },
        });
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETING_REQUEST",
          BodyLocKey = "MEETING_REQUEST_EXPIRED",
        },
        new PushNotificationData
        {
          Action = "MeetingRequestExpired",
          RedirectTo = "meeting",
        });
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETING",
          BodyLocKey = "MEETING_EXPIRED",
        },
        new PushNotificationData
        {
          Action = "MeetingExpired",
          RedirectTo = "meeting",
        });
    }

    public async Task BroadcastUserSentFriendRequest(UserSentFriendRequestAction action, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "FRIENDS",
          BodyLocKey = "NEW_FRIEND_REQUEST",
        },
        new PushNotificationData
        {
          Action = "UserSentFriendRequest",
          RedirectTo = "friends",
        });
    }

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestAction action, IEnumerable<int> usersId)
    {
      if (action.IsAccepted)
      {
        await SendNotification(
          usersId,
          new PushNotificationContent
          {
            TitleLocKey = "FRIENDS",
            BodyLocKey = "FRIEND_REQUEST_ACCEPTED",
          },
          new PushNotificationData
          {
            Action = "UserRespondedFriendRequest",
            RedirectTo = "friends",
          });
      }
    }

    private async Task SendNotification(IEnumerable<int> usersId, PushNotificationContent notification, PushNotificationData data = null)
    {
      foreach (var userId in usersId)
      {
        await SendNotification(userId, notification, data);
      }
    }

    private async Task SendNotification(int userId, PushNotificationContent notification, PushNotificationData data = null)
    {
      var message = new PushNotificationMessage($"/topics/user-{userId}", notification, data);
      await HttpClient.PostAsync("send", PrepareData(message));
    }
  }
}
