using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Relations.Events.UserRespondedFriendRequest
{
  public class UserRespondedFriendRequestEventHandler : EventHandler<UserRespondedFriendRequestEvent>
  {
    private readonly INotificationsService _notifications;

    public UserRespondedFriendRequestEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserRespondedFriendRequestEvent request)
    {
      var usersId = new List<int> { request.InvitedUserId, request.InvitingUserId };

      await _notifications.BroadcastUserRespondedFriendRequest(
        new UserRespondedFriendRequestAction(request.RequestId, request.IsAccepted, usersId));

      return Unit.Value;
    }
  }
}
