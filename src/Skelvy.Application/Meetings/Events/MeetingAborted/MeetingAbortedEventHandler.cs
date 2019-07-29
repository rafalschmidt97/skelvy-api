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
    private readonly IGroupUsersRepository _groupUsersRepository;

    public MeetingAbortedEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(MeetingAbortedEvent request)
    {
      var meetingUsers = await _groupUsersRepository
        .FindAllWithRemovedAfterOrEqualAbortedAtByGroupId(request.MeetingId, request.UserLeftAt);

      var broadcastUsersId = meetingUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingAborted(new MeetingAbortedAction(request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
