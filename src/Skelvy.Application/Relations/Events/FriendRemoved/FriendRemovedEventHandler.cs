using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Relations.Events.FriendRemoved
{
  public class FriendRemovedEventHandler : EventHandler<FriendRemovedEvent>
  {
    private readonly INotificationsService _notifications;

    public FriendRemovedEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(FriendRemovedEvent removed)
    {
      var usersId = new List<int> { removed.RemovedUserId };

      await _notifications.BroadcastFriendRemoved(
        new FriendRemovedNotification(removed.RemovingUserId, removed.RemovedUserId, usersId));

      return Unit.Value;
    }
  }
}
