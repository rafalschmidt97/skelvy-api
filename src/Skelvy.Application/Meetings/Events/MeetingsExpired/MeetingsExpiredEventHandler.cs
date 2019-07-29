using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.MeetingExpired
{
  public class MeetingsExpiredEventHandler : EventHandler<MeetingsExpiredEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public MeetingsExpiredEventHandler(
      INotificationsService notifications,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(MeetingsExpiredEvent request)
    {
      foreach (var meetingId in request.MeetingsId)
      {
        var meetingUsers = await _groupUsersRepository.FindAllWithExpiredByGroupId(meetingId);
        var broadcastUsersId = meetingUsers.Select(x => x.UserId).ToList();
        await _notifications.BroadcastMeetingExpired(new MeetingExpiredAction(broadcastUsersId));
      }

      return Unit.Value;
    }
  }
}
