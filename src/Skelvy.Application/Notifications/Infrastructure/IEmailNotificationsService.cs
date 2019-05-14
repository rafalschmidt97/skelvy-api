using System.Threading.Tasks;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface IEmailNotificationsService
  {
    Task BroadcastUserCreated(UserCreatedAction action);
    Task BroadcastUserRemoved(UserRemovedAction action);
    Task BroadcastUserDisabled(UserDisabledAction action);
  }
}
