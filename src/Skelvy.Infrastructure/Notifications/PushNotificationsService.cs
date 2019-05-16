using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications.Infrastructure;

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

    public async Task BroadcastUserSentMeetingChatMessage(UserSentMessageAction action, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        Title = action.UserName,
        Body = action.Message,
      });
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        TitleLocKey = "MEETING",
        BodyLocKey = "USER_JOINED_MEETING",
      });
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        TitleLocKey = "MEETING",
        BodyLocKey = "USER_FOUND_MEETING",
      });
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        TitleLocKey = "MEETING",
        BodyLocKey = "USER_LEFT_MEETING",
      });
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        TitleLocKey = "MEETING_REQUEST",
        BodyLocKey = "MEETING_REQUEST_EXPIRED",
      });
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action, IEnumerable<int> usersId)
    {
      await SendNotification(usersId, new PushNotificationContent
      {
        TitleLocKey = "MEETING",
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
