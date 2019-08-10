using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Users.Events.UserDisabled
{
  public class UserDisabledEventHandler : EventHandler<UserDisabledEvent>
  {
    private readonly INotificationsService _notifications;

    public UserDisabledEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserDisabledEvent request)
    {
      await _notifications.BroadcastUserDisabled(
        new UserDisabledNotification(request.UserId, request.Reason, request.Email, request.Language));

      return Unit.Value;
    }
  }
}
