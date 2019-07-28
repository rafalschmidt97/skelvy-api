using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;

namespace Skelvy.Application.Meetings.Events.MeetingExpired
{
  public class MeetingRequestsExpiredEventHandler : EventHandler<MeetingRequestsExpiredEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;

    public MeetingRequestsExpiredEventHandler(INotificationsService notifications, IMeetingRequestsRepository meetingRequestsRepository)
    {
      _notifications = notifications;
      _meetingRequestsRepository = meetingRequestsRepository;
    }

    public override async Task<Unit> Handle(MeetingRequestsExpiredEvent request)
    {
      foreach (var requestId in request.RequestsId)
      {
        var meetingRequest = await _meetingRequestsRepository.FindOneWithExpiredById(requestId);
        var usersId = new List<int> { meetingRequest.UserId };
        await _notifications.BroadcastMeetingRequestExpired(new MeetingRequestExpiredAction(usersId));
      }

      return Unit.Value;
    }
  }
}
