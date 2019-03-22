using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveUserCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      await RemoveFromMeeting(user, cancellationToken);

      _context.Users.Remove(user);
      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }

    private async Task RemoveFromMeeting(User user, CancellationToken cancellationToken)
    {
      var meetingUser = await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

      if (meetingUser != null)
      {
        var meeting = await _context.Meetings
          .Include(x => x.Users)
          .ThenInclude(x => x.User)
          .ThenInclude(x => x.MeetingRequest)
          .FirstOrDefaultAsync(x => x.Id == meetingUser.MeetingId, cancellationToken);

        _context.MeetingUsers.Remove(meetingUser);
        var meetingUserDetails = meeting.Users.First(x => x.UserId == meetingUser.UserId);
        _context.MeetingRequests.Remove(meetingUserDetails.User.MeetingRequest);

        if (meeting.Users.Count == 2)
        {
          meeting.Users.First(x => x.UserId != meetingUser.UserId).User.MeetingRequest.Status = MeetingStatusTypes.Searching;
          _context.Meetings.Remove(meeting);
        }

        await _context.SaveChangesAsync(cancellationToken);
        await BroadcastUserLeftMeeting(meetingUser, meeting.Users, cancellationToken);
      }
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
