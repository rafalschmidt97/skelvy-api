using System;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Domain.Entities;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailNotificationsService : IEmailNotificationsService
  {
    public Task BroadcastUserCreated(User user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task BroadcastUserDeleted(User user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
