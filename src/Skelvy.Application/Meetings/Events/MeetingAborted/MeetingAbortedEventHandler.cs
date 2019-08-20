using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
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
      var groupUsers = await _groupUsersRepository
        .FindAllWithRemovedAfterOrEqualAbortedAtByGroupId(request.GroupId, request.UserLeftAt);

      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingAborted(new MeetingAbortedNotification(request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
