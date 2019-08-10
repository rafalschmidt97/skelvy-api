using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserJoinedMeeting
{
  public class UserJoinedMeetingEventHandler : EventHandler<UserJoinedMeetingEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public UserJoinedMeetingEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(UserJoinedMeetingEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.MeetingId);
      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserJoinedMeeting(new UserJoinedMeetingNotification(request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
