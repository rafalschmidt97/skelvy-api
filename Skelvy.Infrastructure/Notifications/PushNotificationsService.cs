using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Core.Exceptions;
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
      if (message.RegistrationIds.Count > 0)
      {
        await HttpClient.PostAsync("send", PrepareData(message), cancellationToken);
      }
    }

    public async Task<PushVerification> VerifyIds(ICollection<string> registrationIds, CancellationToken cancellationToken)
    {
      var message = new PushNotificationMessage
      {
        RegistrationIds = registrationIds,
        DryRun = true
      };

      var response = await HttpClient.PostAsync("send", PrepareData(message), cancellationToken);

      if (!response.IsSuccessStatusCode)
      {
        throw new ConflictException($"Firebase problem with entity {nameof(PushNotificationMessage)}).");
      }

      return await GetData<PushVerification>(response.Content);
    }
  }
}
