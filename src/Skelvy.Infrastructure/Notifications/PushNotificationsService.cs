using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Domain.Entities;

namespace Skelvy.Infrastructure.Notifications
{
  public class PushNotificationsService : HttpServiceBase, IPushNotificationsService
  {
    public PushNotificationsService(IConfiguration configuration)
      : base("https://fcm.googleapis.com/fcm/")
    {
      HttpClient.DefaultRequestHeaders
        .TryAddWithoutValidation("Authorization", "key=" + configuration["Google:KeyWeb"]);
    }

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> userIds)
    {
      var notification = new PushNotificationContent
      {
        Body = message.Message,
      };

      await SendNotifications(userIds, notification);
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "USER_JOINED_MEETING",
      };

      await SendNotifications(userIds, notification);
    }

    public async Task BroadcastUserFoundMeeting(IEnumerable<int> userIds)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "USER_FOUND_MEETING",
      };

      await SendNotifications(userIds, notification);
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "USER_LEFT_MEETING",
      };

      await SendNotifications(userIds, notification);
    }

    public async Task BroadcastMeetingRequestExpired(IEnumerable<int> userIds)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "MEETING_REQUEST_EXPIRED",
      };

      await SendNotifications(userIds, notification);
    }

    public async Task BroadcastMeetingExpired(IEnumerable<int> userIds)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "MEETING_EXPIRED",
      };

      await SendNotifications(userIds, notification);
    }

    private async Task SendNotifications(IEnumerable<int> userIds, PushNotificationContent notification)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification);
      }
    }

    private async Task SendNotification(int userId, PushNotificationContent notification)
    {
      var message = new PushNotificationMessage
      {
        To = $"/topics/user-{userId}",
        Notification = notification,
      };

      await HttpClient.PostAsync("send", PrepareData(message));
    }
  }
}
