using System.Threading.Tasks;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface IEmailNotificationsService
  {
    Task BroadcastUserCreated(UserCreatedNotification notification);
    Task BroadcastUserRemoved(UserRemovedNotification notification);
    Task BroadcastUserDisabled(UserDisabledNotification notification);
  }
}
