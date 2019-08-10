using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UsersConnectedToMeeting
{
  public class UsersConnectedToMeetingEventHandler : EventHandler<UsersConnectedToMeetingEvent>
  {
    private readonly INotificationsService _notifications;

    public UsersConnectedToMeetingEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UsersConnectedToMeetingEvent request)
    {
      var usersId = new List<int> { request.User1Id, request.User2Id };
      await _notifications.BroadcastUserFoundMeeting(new UserFoundMeetingNotification(usersId));

      return Unit.Value;
    }
  }
}
