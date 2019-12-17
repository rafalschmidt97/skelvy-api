using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.UserRespondedMeetingInvitation
{
  public class UserRespondedMeetingInvitationEventHandler : EventHandler<UserRespondedMeetingInvitationEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public UserRespondedMeetingInvitationEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(UserRespondedMeetingInvitationEvent request)
    {
      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);
      var broadcastUsersId = groupUsers.Where(x => x.UserId != request.InvitedUserId).Select(x => x.UserId).ToList();

      await _notifications.BroadcastUserRespondedMeetingInvitation(
          new UserRespondedMeetingInvitationNotification(
            request.InvitationId,
            request.IsAccepted,
            request.InvitingUserId,
            request.InvitedUserId,
            request.MeetingId,
            request.GroupId,
            broadcastUsersId));

      return Unit.Value;
    }
  }
}
