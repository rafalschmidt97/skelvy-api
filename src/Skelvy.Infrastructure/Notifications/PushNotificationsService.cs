using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
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

    public async Task BroadcastUserSentMessage(UserSentMessageNotification notification, IEnumerable<int> usersId)
    {
      var data = new PushNotificationData
      {
        Action = "UserSentMessage",
        RedirectTo = "chat",
        Data = notification.Messages,
      };

      if (notification.Message.Text != null)
      {
        await SendNotification(
          usersId,
          new PushNotificationContent
          {
            Title = notification.Message.UserName,
            Body = notification.Message.Text,
          },
          data);
      }
      else
      {
        if (notification.Message.AttachmentUrl != null)
        {
          await SendNotification(
            usersId,
            new PushNotificationContent
            {
              Title = notification.Message.UserName,
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
              Title = notification.Message.UserName,
              BodyLocKey = "USER_SENT_MESSAGE",
            },
            data);
        }
      }
    }

    public async Task BroadcastUserJoinedGroup(UserJoinedGroupNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "GROUP",
          BodyLocKey = "USER_JOINED_GROUP",
        },
        new PushNotificationData
        {
          Action = "UserJoinedGroup",
          RedirectTo = "group",
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification, IEnumerable<int> usersId)
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

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification, IEnumerable<int> usersId)
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
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedNotification notification, IEnumerable<int> usersId)
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
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastGroupAborted(GroupAbortedNotification notification, IList<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "GROUP",
          BodyLocKey = "GROUP_ABORTED",
        },
        new PushNotificationData
        {
          Action = "GroupAborted",
          RedirectTo = "group",
          Data = new { notification.UserId },
        });
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification, IEnumerable<int> usersId)
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

    public async Task BroadcastMeetingExpired(MeetingExpiredNotification notification, IEnumerable<int> usersId)
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

    public async Task BroadcastUserSentFriendRequest(UserSentFriendRequestNotification notification, IEnumerable<int> usersId)
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

    public async Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestNotification notification, IEnumerable<int> usersId)
    {
      if (notification.IsAccepted)
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
