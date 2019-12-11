using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserRespondedMeetingInvitation
{
  public class UserRespondedMeetingInvitationEventHandler : EventHandler<UserRespondedMeetingInvitationEvent>
  {
    private readonly INotificationsService _notifications;

    public UserRespondedMeetingInvitationEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserRespondedMeetingInvitationEvent request)
    {
      var usersId = new List<int> { request.InvitingUserId };

      await _notifications.BroadcastUserRespondedMeetingInvitation(
        new UserRespondedMeetingInvitationNotification(request.InvitationId, request.IsAccepted, usersId));

      return Unit.Value;
    }
  }
}
