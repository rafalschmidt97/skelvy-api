using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.MeetingUserRoleUpdated
{
  public class MeetingUserRoleUpdatedEventHandler : EventHandler<MeetingUserRoleUpdatedEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public MeetingUserRoleUpdatedEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(MeetingUserRoleUpdatedEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);

      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingUserRoleUpdated(
        new MeetingUserRoleUpdatedNotification(
          request.MeetingId,
          request.GroupId,
          request.UserId,
          request.UpdatedUserId,
          request.Role,
          broadcastUsersId));

      return Unit.Value;
    }
  }
}
