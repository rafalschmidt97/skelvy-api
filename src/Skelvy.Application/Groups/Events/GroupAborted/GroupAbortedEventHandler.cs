using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Notifications;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Groups.Events.GroupAborted
{
  public class GroupAbortedEventHandler : EventHandler<GroupAbortedEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public GroupAbortedEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(GroupAbortedEvent request)
    {
      var groupUsers = await _groupUsersRepository
        .FindAllWithRemovedAfterOrEqualAbortedAtByGroupId(request.GroupId, request.UserLeftAt);

      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastGroupAborted(new GroupAbortedNotification(request.GroupId, request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
