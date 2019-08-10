using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Relations.Events.UserSentFriendRequest
{
  public class UserSentFriendRequestEventHandler : EventHandler<UserSentFriendRequestEvent>
  {
    private readonly INotificationsService _notifications;

    public UserSentFriendRequestEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserSentFriendRequestEvent request)
    {
      var usersId = new List<int> { request.InvitedUserId };
      await _notifications.BroadcastUserSentFriendRequest(new UserSentFriendRequestNotification(request.RequestId, usersId));
      return Unit.Value;
    }
  }
}
