using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests
{
  public class RemoveExpiredMeetingRequestsCommandHandler
    : IRequestHandler<RemoveExpiredMeetingRequestsCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveExpiredMeetingRequestsCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(
      RemoveExpiredMeetingRequestsCommand request,
      CancellationToken cancellationToken)
    {
      var today = DateTimeOffset.UtcNow;
      var requestsToRemove = await _context.MeetingRequests
        .Where(x => x.MaxDate < today && x.Status == MeetingStatusTypes.Searching)
        .ToListAsync(cancellationToken);

      var isDataChanged = false;

      if (requestsToRemove.Count != 0)
      {
        _context.MeetingRequests.RemoveRange(requestsToRemove);
        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync(cancellationToken);
        await BroadcastMeetingRequestExpired(requestsToRemove, cancellationToken);
      }

      return Unit.Value;
    }

    private async Task BroadcastMeetingRequestExpired(IEnumerable<MeetingRequest> requestsToRemove, CancellationToken cancellationToken)
    {
      var userIds = requestsToRemove.Select(x => x.UserId).ToList();
      await _notifications.BroadcastMeetingRequestExpired(userIds, cancellationToken);
    }
  }
}
