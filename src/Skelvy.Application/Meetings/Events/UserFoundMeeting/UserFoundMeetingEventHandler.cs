using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserFoundMeeting
{
  public class UserFoundMeetingEventHandler : EventHandler<UserFoundMeetingEvent>
  {
    private readonly INotificationsService _notifications;

    public UserFoundMeetingEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserFoundMeetingEvent request)
    {
      var usersId = new List<int> { request.UserId };
      await _notifications.BroadcastUserFoundMeeting(new UserFoundMeetingAction(usersId));

      return Unit.Value;
    }
  }
}
