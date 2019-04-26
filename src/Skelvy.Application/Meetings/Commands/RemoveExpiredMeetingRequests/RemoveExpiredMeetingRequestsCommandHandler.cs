using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests
{
  public class RemoveExpiredMeetingRequestsCommandHandler : CommandHandler<RemoveExpiredMeetingRequestsCommand>
  {
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly INotificationsService _notifications;

    public RemoveExpiredMeetingRequestsCommandHandler(IMeetingRequestsRepository meetingRequestsRepository, INotificationsService notifications)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingRequestsCommand request)
    {
      var today = DateTimeOffset.UtcNow;
      var requestsToRemove = await _meetingRequestsRepository.FindAllSearchingAfterMaxDate(today);

      var isDataChanged = false;

      if (requestsToRemove.Count != 0)
      {
        requestsToRemove.ForEach(x => x.Expire());
        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _meetingRequestsRepository.Context.SaveChangesAsync();
        await BroadcastMeetingRequestExpired(requestsToRemove);
      }

      return Unit.Value;
    }

    private async Task BroadcastMeetingRequestExpired(IEnumerable<MeetingRequest> requestsToRemove)
    {
      var usersId = requestsToRemove.Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingRequestExpired(usersId);
    }
  }
}
