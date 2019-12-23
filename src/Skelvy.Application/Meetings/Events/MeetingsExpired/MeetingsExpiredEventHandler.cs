using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.MeetingsExpired
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
        var groupUsers = await _groupUsersRepository.FindAllWithExpiredByGroupId(meetingId);

        if (groupUsers.Count > 0)
        {
          var groupId = groupUsers.Select(x => x.GroupId).First();
          var broadcastUsersId = groupUsers.Select(x => x.UserId).ToList();
          await _notifications.BroadcastMeetingExpired(new MeetingExpiredNotification(meetingId, groupId, broadcastUsersId));
        }
      }

      return Unit.Value;
    }
  }
}
