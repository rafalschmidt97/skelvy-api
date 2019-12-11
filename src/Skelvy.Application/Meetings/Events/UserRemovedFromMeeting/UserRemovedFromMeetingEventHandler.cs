using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserRemovedFromMeeting
{
  public class UserRemovedFromMeetingEventHandler : EventHandler<UserRemovedFromMeetingEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public UserRemovedFromMeetingEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(UserRemovedFromMeetingEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);
      var broadcastUsersId = groupUsers
        .Where(x => x.UserId != request.UserId && x.UserId != request.RemovedUserId).Select(x => x.UserId).ToList();
      var broadcastRemovedUserId = new List<int> { request.RemovedUserId };

      await _notifications.BroadcastUserRemovedFromMeeting(
        new UserRemovedFromMeetingNotification(request.UserId, request.RemovedUserId, request.GroupId, request.MeetingId, broadcastUsersId));

      await _notifications.BroadcastUserSelfRemovedFromMeeting(
        new UserRemovedFromMeetingNotification(request.UserId, request.RemovedUserId, request.GroupId, request.MeetingId, broadcastRemovedUserId));

      return Unit.Value;
    }
  }
}
