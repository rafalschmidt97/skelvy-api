using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Notifications;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Groups.Events.UserRemovedFromGroup
{
  public class UserRemovedFromGroupEventHandler : EventHandler<UserRemovedFromGroupEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public UserRemovedFromGroupEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(UserRemovedFromGroupEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);
      var broadcastUsersId = groupUsers
        .Where(x => x.UserId != request.UserId && x.UserId != request.RemovedUserId).Select(x => x.UserId).ToList();
      var broadcastRemovedUserId = new List<int> { request.RemovedUserId };

      await _notifications.BroadcastUserRemovedFromGroup(
        new UserRemovedFromGroupNotification(request.UserId, request.RemovedUserId, request.GroupId, broadcastUsersId));

      await _notifications.BroadcastUserSelfRemovedFromGroup(
        new UserRemovedFromGroupNotification(request.UserId, request.RemovedUserId, request.GroupId, broadcastRemovedUserId));

      return Unit.Value;
    }
  }
}
