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

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings
{
  public class RemoveExpiredMeetingsCommandHandler : CommandHandler<RemoveExpiredMeetingsCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveExpiredMeetingsCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingsCommand request)
    {
      var today = DateTimeOffset.UtcNow;
      var meetingsToRemove = await _context.Meetings
        .Include(x => x.Users)
        .Where(x => x.Date < today && !x.IsRemoved)
        .ToListAsync();

      var isDataChanged = false;

      if (meetingsToRemove.Count != 0)
      {
        var meetingUsers = meetingsToRemove.SelectMany(x => x.Users);
        var meetingRequests = await _context.MeetingRequests
          .Where(x => x.Status == MeetingRequestStatusTypes.Found && !x.IsRemoved)
          .ToListAsync();

        var meetingRequestsToRemove = meetingRequests.Where(x => meetingUsers.Any(y => y.UserId == x.UserId)).ToList();

        meetingsToRemove.ForEach(x => x.Expire());
        meetingRequestsToRemove.ForEach(x => x.Expire());

        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync();
        await BroadcastMeetingExpired(meetingsToRemove);
      }

      return Unit.Value;
    }

    private async Task BroadcastMeetingExpired(IEnumerable<Meeting> meetingsToRemove)
    {
      foreach (var meeting in meetingsToRemove)
      {
        var userIds = meeting.Users.Select(x => x.UserId).ToList();
        await _notifications.BroadcastMeetingExpired(userIds);
      }
    }
  }
}
