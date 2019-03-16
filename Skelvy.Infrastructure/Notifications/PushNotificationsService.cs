using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Core.Infrastructure.Notifications;

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

    public async Task BroadcastMessage(PushNotificationMessage message, CancellationToken cancellationToken)
    {
      if (message.RegistrationIds.Count <= 0)
      {
        return;
      }

      await HttpClient.PostAsync("send", PrepareData(message), cancellationToken);
    }
  }
}
