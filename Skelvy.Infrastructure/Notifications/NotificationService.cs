using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Infrastructure;

namespace Skelvy.Infrastructure.Notifications
{
  public class NotificationService : INotificationService
  {
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
      _logger = logger;
    }

    public Task Send(string message)
    {
      _logger.LogInformation(message);

      return Task.CompletedTask;
    }
  }
}
