using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Core.Infrastructure.Notifications;
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
      foreach (var userId in userIds)
      {
        await SendNotification(userId, null, message.Message, cancellationToken);
      }
    }

    public async Task BroadcastUserJoinedMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, null, "A new user has been added to the group", cancellationToken);
      }
    }

    public async Task BroadcastUserFoundMeeting(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, null, "A new meeting has been found", cancellationToken);
      }
    }

    public async Task BroadcastUserLeftMeeting(MeetingUser user, ICollection<int> userIds, CancellationToken cancellationToken)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, null, "A user has left the group", cancellationToken);
      }
    }

    public async Task BroadcastMeetingRequestExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, null, "A meeting request has expired", cancellationToken);
      }
    }

    public async Task BroadcastMeetingExpired(ICollection<int> userIds, CancellationToken cancellationToken)
    {
      foreach (var userId in userIds)
      {
        await SendNotification(userId, null, "A meeting has expired", cancellationToken);
      }
    }

    private async Task SendNotification(int userId, string title, string body, CancellationToken cancellationToken)
    {
      var message = new PushNotificationMessage
      {
        To = $"/topics/user-{userId}",
        Notification = new PushNotificationContent
        {
          Title = title,
          Body = body
        }
      };

      await HttpClient.PostAsync("send", PrepareData(message), cancellationToken);
    }
  }
}
