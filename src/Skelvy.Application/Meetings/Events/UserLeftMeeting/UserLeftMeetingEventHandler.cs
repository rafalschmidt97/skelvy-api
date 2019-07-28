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
    private readonly IMeetingUsersRepository _meetingUsersRepository;

    public UserLeftMeetingEventHandler(
      INotificationsService notifications,
      IMeetingUsersRepository meetingUsersRepository)
    {
      _notifications = notifications;
      _meetingUsersRepository = meetingUsersRepository;
    }

    public override async Task<Unit> Handle(UserLeftMeetingEvent request)
    {
      var meetingUsers = await _meetingUsersRepository.FindAllByMeetingId(request.MeetingId);
      var broadcastUsersId = meetingUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(new UserLeftMeetingAction(request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
