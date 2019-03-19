using System.Threading;
using System.Threading.Tasks;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public interface IPushNotificationsService
  {
    Task BroadcastMessage(PushNotificationMessage message, CancellationToken cancellationToken);
  }
}
