using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserLeftMeeting
{
  public class UserLeftMeetingEventHandler : EventHandler<UserLeftMeetingEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public UserLeftMeetingEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(UserLeftMeetingEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);
      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();

      await _notifications.BroadcastUserLeftMeeting(
        new UserLeftMeetingNotification(request.UserId, request.GroupId, request.MeetingId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
