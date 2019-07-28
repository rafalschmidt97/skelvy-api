using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Auth.Events.UserCreated
{
  public class UserCreatedEventHandler : EventHandler<UserCreatedEvent>
  {
    private readonly INotificationsService _notifications;

    public UserCreatedEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserCreatedEvent request)
    {
      await _notifications.BroadcastUserCreated(new UserCreatedAction(request.UserId, request.Email, request.Language));
      return Unit.Value;
    }
  }
}
