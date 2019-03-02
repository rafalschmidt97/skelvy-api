using System.Threading;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task SendMessage(MeetingChatMessage message, CancellationToken cancellationToken);
  }
}
