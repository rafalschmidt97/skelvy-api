using System.Threading;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Notifications
{
  public interface IEmailNotificationsService
  {
    Task BroadcastUserCreated(User user, CancellationToken cancellationToken);
    Task BroadcastUserDeleted(User user, CancellationToken cancellationToken);
    Task BroadcastUserDisabled(User user, string reason, CancellationToken cancellationToken);
  }
}
