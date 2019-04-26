using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Notifications;
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
      await SendNotification(userIds, new PushNotificationContent
      {
        Body = message.Message,
      });
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      await SendNotification(userIds, new PushNotificationContent
      {
        BodyLocKey = "USER_JOINED_MEETING",
      });
    }

    public async Task BroadcastUserFoundMeeting(IEnumerable<int> userIds)
    {
      await SendNotification(userIds, new PushNotificationContent
      {
        BodyLocKey = "USER_FOUND_MEETING",
      });
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> userIds)
    {
      await SendNotification(userIds, new PushNotificationContent
      {
        BodyLocKey = "USER_LEFT_MEETING",
      });
    }

    public async Task BroadcastMeetingRequestExpired(IEnumerable<int> userIds)
    {
      await SendNotification(userIds, new PushNotificationContent
      {
        BodyLocKey = "MEETING_REQUEST_EXPIRED",
      });
    }

    public async Task BroadcastMeetingExpired(IEnumerable<int> userIds)
    {
      await SendNotification(userIds, new PushNotificationContent
      {
        BodyLocKey = "MEETING_EXPIRED",
      });
    }

    private async Task SendNotification(IEnumerable<int> userIds, PushNotificationContent notification)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, notification);
      }
    }

    private async Task SendNotification(int userId, PushNotificationContent notification)
    {
      var message = new PushNotificationMessage($"/topics/user-{userId}", notification);
      await HttpClient.PostAsync("send", PrepareData(message));
    }
  }
}
