using System.Collections.Generic;
using System.Threading;
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
        .TryAddWithoutValidation("Authorization", "key=" + configuration["Google:Key"]);
    }

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var notification = new PushNotificationContent
      {
        Body = message.Message
      };

      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification, cancellationToken);
      }
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "USER_JOINED_MEETING"
      };

      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification, cancellationToken);
      }
    }

    public async Task BroadcastUserFoundMeeting(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "USER_FOUND_MEETING"
      };

      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification, cancellationToken);
      }
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "USER_LEFT_MEETING"
      };

      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification, cancellationToken);
      }
    }

    public async Task BroadcastMeetingRequestExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "MEETING_REQUEST_EXPIRED"
      };

      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification, cancellationToken);
      }
    }

    public async Task BroadcastMeetingExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      var notification = new PushNotificationContent
      {
        BodyLocKey = "MEETING_EXPIRED"
      };

      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification, cancellationToken);
      }
    }

    private async Task SendNotification(int userId, PushNotificationContent notification, CancellationToken cancellationToken)
    {
      var message = new PushNotificationMessage
      {
        To = $"/topics/user-{userId}",
        Notification = notification
      };

      await HttpClient.PostAsync("send", PrepareData(message), cancellationToken);
    }
  }
}
