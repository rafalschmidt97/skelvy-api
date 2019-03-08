using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveEmptyMeetings
{
  public class RemoveEmptyMeetingsCommandHandler : IRequestHandler<RemoveEmptyMeetingsCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveEmptyMeetingsCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(RemoveEmptyMeetingsCommand request, CancellationToken cancellationToken)
    {
      var meetings = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.MeetingRequest)
        .ToListAsync(cancellationToken);
      var isDataChanged = false;
      var removedMeetings = new List<Meeting>();

      foreach (var meeting in meetings)
      {
        if (meeting.Users.Count <= 1)
        {
          RemoveMeeting(meeting);
          removedMeetings.Add(meeting);
          isDataChanged = true;
        }
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var removedMeeting in removedMeetings)
        {
          await _notifications.BroadcastUserLeftMeeting(removedMeeting.Id, cancellationToken);
        }
      }

      return Unit.Value;
    }

    private void RemoveMeeting(Meeting meeting)
    {
      if (meeting.Users.Count == 1)
      {
        meeting.Users.First().User.MeetingRequest.Status = MeetingStatusTypes.Searching;
      }

      _context.Meetings.Remove(meeting);
    }
  }
}
