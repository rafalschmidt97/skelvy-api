using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Relations.Events.UserRespondedFriendInvitation
{
  public class UserRespondedFriendInvitationEventHandler : EventHandler<UserRespondedFriendInvitationEvent>
  {
    private readonly INotificationsService _notifications;

    public UserRespondedFriendInvitationEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserRespondedFriendInvitationEvent invitation)
    {
      var usersId = new List<int> { invitation.InvitedUserId, invitation.InvitingUserId };

      await _notifications.BroadcastUserRespondedFriendInvitation(
        new UserRespondedFriendInvitationNotification(invitation.InvitationId, invitation.IsAccepted, usersId));

      return Unit.Value;
    }
  }
}
