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

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings
{
  public class RemoveExpiredMeetingsCommandHandler
    : IRequestHandler<RemoveExpiredMeetingsCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveExpiredMeetingsCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(
      RemoveExpiredMeetingsCommand request,
      CancellationToken cancellationToken)
    {
      var today = DateTimeOffset.UtcNow;
      var meetingsToRemove = await _context.Meetings
        .Include(x => x.Users)
        .Where(x => x.Date < today.AddDays(-1))
        .ToListAsync(cancellationToken);

      var isDataChanged = false;

      if (meetingsToRemove.Count != 0)
      {
        var meetingsToRemoveUserIds = meetingsToRemove.SelectMany(x => x.Users);
        var meetingRequests = await _context.MeetingRequests.Where(x => x.Status == MeetingStatusTypes.Found).ToListAsync(cancellationToken);
        var meetingRequestsToRemove = meetingRequests.Where(x => meetingsToRemoveUserIds.Any(y => y.UserId == x.UserId)).ToList();

        _context.Meetings.RemoveRange(meetingsToRemove);
        _context.MeetingRequests.RemoveRange(meetingRequestsToRemove);

        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync(cancellationToken);
        await BroadcastMeetingExpired(meetingsToRemove, cancellationToken);
      }

      return Unit.Value;
    }

    private async Task BroadcastMeetingExpired(IEnumerable<Meeting> meetingsToRemove, CancellationToken cancellationToken)
    {
      foreach (var meeting in meetingsToRemove)
      {
        var userIds = meeting.Users.Select(x => x.UserId).ToList();
        await _notifications.BroadcastMeetingExpired(userIds, cancellationToken);
      }
    }
  }
}
