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
        .TryAddWithoutValidation("Authorization", "key=" + configuration["SKELVY_GOOGLE_KEY_WEB"]);
    }

    public async Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        Body = message.Message,
      });
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        BodyLocKey = "USER_JOINED_MEETING",
      });
    }

    public async Task BroadcastUserFoundMeeting(IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        BodyLocKey = "USER_FOUND_MEETING",
      });
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        BodyLocKey = "USER_LEFT_MEETING",
      });
    }

    public async Task BroadcastMeetingRequestExpired(IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        BodyLocKey = "MEETING_REQUEST_EXPIRED",
      });
    }

    public async Task BroadcastMeetingExpired(IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        BodyLocKey = "MEETING_EXPIRED",
      });
    }

    private async Task SendNotification(IEnumerable<int> usersId, PushNotificationContent notification)
    {
      foreach (var userId in usersId)
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
