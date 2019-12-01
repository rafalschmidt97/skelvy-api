using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.MeetingUpdated
{
  public class MeetingUpdatedEventHandler : EventHandler<MeetingUpdatedEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public MeetingUpdatedEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(MeetingUpdatedEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);

      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingUpdated(new MeetingUpdatedNotification(request.MeetingId, request.GroupId, request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
