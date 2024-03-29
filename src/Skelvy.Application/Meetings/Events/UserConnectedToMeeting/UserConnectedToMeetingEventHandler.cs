using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserConnectedToMeeting
{
  public class UserConnectedToMeetingEventHandler : EventHandler<UserConnectedToMeetingEvent>
  {
    private readonly INotificationsService _notifications;

    public UserConnectedToMeetingEventHandler(INotificationsService notifications)
    {
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(UserConnectedToMeetingEvent request)
    {
      var usersId = new List<int> { request.UserId };
      await _notifications.BroadcastUserConnectedToMeeting(new UserConnectedToMeetingNotification(request.MeetingId, request.RequestId, request.GroupId, usersId));

      return Unit.Value;
    }
  }
}
