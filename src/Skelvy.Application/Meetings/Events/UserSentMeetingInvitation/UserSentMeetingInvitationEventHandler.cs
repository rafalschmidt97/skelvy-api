using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserSentMeetingInvitation
{
  public class UserSentMeetingInvitationEventHandler : EventHandler<UserSentMeetingInvitationEvent>
  {
    private readonly INotificationsService _notifications;

    public UserSentMeetingInvitationEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserSentMeetingInvitationEvent request)
    {
      var usersId = new List<int> { request.InvitedUserId };
      await _notifications.BroadcastUserSentMeetingInvitation(new UserSentMeetingInvitationNotification(request.InvitationId, usersId));
      return Unit.Value;
    }
  }
}
