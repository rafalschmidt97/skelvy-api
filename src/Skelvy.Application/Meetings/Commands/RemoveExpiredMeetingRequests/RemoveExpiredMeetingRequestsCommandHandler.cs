using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests
{
  public class RemoveExpiredMeetingRequestsCommandHandler : CommandHandler<RemoveExpiredMeetingRequestsCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveExpiredMeetingRequestsCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingRequestsCommand request)
    {
      var today = DateTimeOffset.UtcNow;
      var requestsToRemove = await _context.MeetingRequests
        .Where(x => x.MaxDate < today && x.Status == MeetingRequestStatusTypes.Searching && !x.IsRemoved)
        .ToListAsync();

      var isDataChanged = false;

      if (requestsToRemove.Count != 0)
      {
        requestsToRemove.ForEach(x => x.Expire());
        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync();
        await BroadcastMeetingRequestExpired(requestsToRemove);
      }

      return Unit.Value;
    }

    private async Task BroadcastMeetingRequestExpired(IEnumerable<MeetingRequest> requestsToRemove)
    {
      var userIds = requestsToRemove.Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingRequestExpired(userIds);
    }
  }
}
