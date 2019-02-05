using System.Threading.Tasks;

namespace Skelvy.Application.Core.Infrastructure
{
  public interface INotificationService
  {
    Task Send(string message);
  }
}
