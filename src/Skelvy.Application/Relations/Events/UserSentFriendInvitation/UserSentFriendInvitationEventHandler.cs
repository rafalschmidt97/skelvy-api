using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Relations.Events.UserSentFriendInvitation
{
  public class UserSentFriendInvitationEventHandler : EventHandler<UserSentFriendInvitationEvent>
  {
    private readonly INotificationsService _notifications;

    public UserSentFriendInvitationEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserSentFriendInvitationEvent invitation)
    {
      var usersId = new List<int> { invitation.InvitedUserId };
      await _notifications.BroadcastUserSentFriendInvitation(new UserSentFriendInvitationNotification(invitation.InvitationId, usersId));
      return Unit.Value;
    }
  }
}
