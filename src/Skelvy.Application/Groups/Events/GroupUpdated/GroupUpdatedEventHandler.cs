using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Notifications;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Groups.Events.GroupUpdated
{
  public class GroupUpdatedEventHandler : EventHandler<GroupUpdatedEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public GroupUpdatedEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(GroupUpdatedEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);

      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastGroupUpdated(new GroupUpdatedNotification(request.GroupId, request.UserId, broadcastUsersId));

      return Unit.Value;
    }
  }
}
