using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.MeetingAborted
{
  public class MeetingAbortedEventHandler : EventHandler<MeetingAbortedEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IMeetingUsersRepository _meetingUsersRepository;

    public MeetingAbortedEventHandler(
      INotificationsService notifications,
      IMeetingUsersRepository meetingUsersRepository)
    {
      _notifications = notifications;
      _meetingUsersRepository = meetingUsersRepository;
    }

    public override async Task<Unit> Handle(MeetingAbortedEvent request)
    {
      var meetingUsers = await _meetingUsersRepository
        .FindAllWithRemovedAfterOrEqualAbortedAtByMeetingId(request.MeetingId, request.UserLeftAt);

      var broadcastUsersId = meetingUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingAborted(new MeetingAbortedAction(request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
