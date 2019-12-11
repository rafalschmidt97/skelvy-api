using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Groups.Infrastructure.Notifications;
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
        RedirectTo = "groups",
        Data = new { notification.Messages, notification.GroupId },
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

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "USER_JOINED_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserJoinedMeeting",
          RedirectTo = "meetings",
          Data = new { notification.UserId, notification.GroupId, notification.MeetingId, notification.UserRole },
        });
    }

    public async Task BroadcastUserConnectedToMeeting(UserConnectedToMeetingNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "USER_CONNECTED_TO_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserConnectedToMeeting",
          RedirectTo = "meetings",
          Data = new { notification.GroupId, notification.MeetingId, notification.RequestId },
        });
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "USER_LEFT_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserLeftMeeting",
          RedirectTo = "meetings",
          Data = new { notification.UserId, notification.GroupId, notification.MeetingId },
        });
    }

    public async Task BroadcastUserRemovedFromMeeting(UserRemovedFromMeetingNotification notification, IList<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "USER_REMOVED_FROM_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserRemovedFromMeeting",
          RedirectTo = "meetings",
          Data = new { notification.UserId, notification.RemovedUserId, notification.GroupId, notification.MeetingId },
        });
    }

    public async Task BroadcastUserSelfRemovedFromMeeting(UserRemovedFromMeetingNotification notification, IList<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "USER_SELF_REMOVED_FROM_MEETING",
        },
        new PushNotificationData
        {
          Action = "UserSelfRemovedFromMeeting",
          RedirectTo = "meetings",
          Data = new { notification.UserId, notification.RemovedUserId, notification.GroupId, notification.MeetingId },
        });
    }

    public async Task BroadcastUserLeftGroup(UserLeftGroupNotification notification, IList<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "GROUPS",
          BodyLocKey = "USER_LEFT_GROUP",
        },
        new PushNotificationData
        {
          Action = "UserLeftGroup",
          RedirectTo = "groups",
          Data = new { notification.UserId, notification.GroupId },
        });
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "MEETING_ABORTED",
        },
        new PushNotificationData
        {
          Action = "MeetingAborted",
          RedirectTo = "meetings",
          Data = new { notification.GroupId, notification.MeetingId },
        });
    }

    public async Task BroadcastGroupAborted(GroupAbortedNotification notification, IList<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "GROUPS",
          BodyLocKey = "GROUP_ABORTED",
        },
        new PushNotificationData
        {
          Action = "GroupAborted",
          RedirectTo = "groups",
          Data = new { notification.GroupId },
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
          RedirectTo = "meetings",
          Data = new { notification.RequestId },
        });
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "MEETING_EXPIRED",
        },
        new PushNotificationData
        {
          Action = "MeetingExpired",
          RedirectTo = "meetings",
          Data = new { notification.MeetingId, notification.GroupId },
        });
    }

    public async Task BroadcastUserSentFriendInvitation(UserSentFriendInvitationNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "FRIENDS",
          BodyLocKey = "NEW_FRIEND_INVITATION",
        },
        new PushNotificationData
        {
          Action = "UserSentFriendInvitation",
          RedirectTo = "friends",
          Data = new { notification.InvitationId },
        });
    }

    public async Task BroadcastUserRespondedFriendInvitation(UserRespondedFriendInvitationNotification notification, IEnumerable<int> usersId)
    {
      if (notification.IsAccepted)
      {
        await SendNotification(
          usersId,
          new PushNotificationContent
          {
            TitleLocKey = "FRIENDS",
            BodyLocKey = "FRIEND_INVITATION_ACCEPTED",
          },
          new PushNotificationData
          {
            Action = "UserRespondedFriendInvitation",
            RedirectTo = "friends",
            Data = new { notification.InvitationId, notification.IsAccepted },
          });
      }
    }

    public async Task BroadcastUserSentMeetingInvitation(UserSentMeetingInvitationNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
        usersId,
        new PushNotificationContent
        {
          TitleLocKey = "MEETINGS",
          BodyLocKey = "NEW_MEETING_INVITATION",
        },
        new PushNotificationData
        {
          Action = "UserSentMeetingInvitation",
          RedirectTo = "meetings",
          Data = new { notification.InvitationId },
        });
    }

    public async Task BroadcastUserRespondedMeetingInvitation(UserRespondedMeetingInvitationNotification notification, IEnumerable<int> usersId)
    {
      await SendNotification(
          usersId,
          new PushNotificationContent
          {
            TitleLocKey = "MEETINGS",
            BodyLocKey = notification.IsAccepted ? "MEETING_INVITATION_ACCEPTED" : "MEETING_INVITATION_DENIED",
          },
          new PushNotificationData
          {
            Action = "UserRespondedMeetingInvitation",
            RedirectTo = "meetings",
            Data = new { notification.InvitationId, notification.IsAccepted },
          });
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
