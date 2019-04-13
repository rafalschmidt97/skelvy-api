using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Notifications
{
  public interface IEmailNotificationsService
  {
    Task BroadcastUserCreated(User user);
    Task BroadcastUserRemoved(User user);
    Task BroadcastUserDisabled(User user, string reason);
  }
}
