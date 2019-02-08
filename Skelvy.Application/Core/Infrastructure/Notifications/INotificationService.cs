using System.Threading.Tasks;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public interface INotificationService
  {
    Task Send(string message);
  }
}
