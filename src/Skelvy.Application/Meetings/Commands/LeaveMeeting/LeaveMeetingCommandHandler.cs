using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommandHandler : IRequestHandler<LeaveMeetingCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public LeaveMeetingCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(LeaveMeetingCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(MeetingUser), request.UserId);
      }

      var meeting = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.MeetingRequest)
        .FirstOrDefaultAsync(x => x.Id == user.MeetingId, cancellationToken);

      var meetingUser = meeting.Users.First(x => x.UserId == user.UserId);
      _context.MeetingUsers.Remove(meetingUser);
      _context.MeetingRequests.Remove(meetingUser.User.MeetingRequest);

      if (meeting.Users.Count == 2)
      {
        meeting.Users.First(x => x.UserId != user.UserId).User.MeetingRequest.Status = MeetingStatusTypes.Searching;
        _context.Meetings.Remove(meeting);
      }

      await _context.SaveChangesAsync(cancellationToken);
      await BroadcastUserLeftMeeting(meetingUser, meeting.Users, cancellationToken);
      return Unit.Value;
    }

    private async Task BroadcastUserLeftMeeting(
      MeetingUser meetingUser,
      IEnumerable<MeetingUser> meetingUsers,
      CancellationToken cancellationToken)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds, cancellationToken);
    }
  }
}
