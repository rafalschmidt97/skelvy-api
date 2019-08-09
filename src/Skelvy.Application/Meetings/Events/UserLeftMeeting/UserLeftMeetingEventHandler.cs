using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
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
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.MeetingId);
      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(new UserLeftMeetingAction(request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
