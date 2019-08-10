using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Users.Events.UserRemoved
{
  public class UserRemovedEventHandler : EventHandler<UserRemovedEvent>
  {
    private readonly INotificationsService _notifications;

    public UserRemovedEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserRemovedEvent request)
    {
      await _notifications.BroadcastUserRemoved(new UserRemovedNotification(request.UserId, request.Email, request.Language));
      return Unit.Value;
    }
  }
}
