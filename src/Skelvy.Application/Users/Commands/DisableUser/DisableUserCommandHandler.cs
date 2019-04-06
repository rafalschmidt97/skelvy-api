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

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommandHandler : IRequestHandler<DisableUserCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public DisableUserCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(DisableUserCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      await LeaveMeetings(user, cancellationToken);

      user.IsDisabled = true;

      await _context.SaveChangesAsync(cancellationToken);
      await _notifications.BroadcastUserDisabled(user, request.Reason, cancellationToken);

      return Unit.Value;
    }

    private async Task LeaveMeetings(User user, CancellationToken cancellationToken)
    {
      var meetingUser = await _context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == MeetingUserStatusTypes.Joined, cancellationToken);

      if (meetingUser != null)
      {
        var meeting = await _context.Meetings
          .Include(x => x.Users)
          .ThenInclude(x => x.User)
          .ThenInclude(x => x.MeetingRequests)
          .FirstOrDefaultAsync(x => x.Id == meetingUser.MeetingId && x.Status == MeetingStatusTypes.Active, cancellationToken);

        meetingUser.Status = MeetingUserStatusTypes.Left;
        var meetingUserDetails = meeting.Users.First(x => x.UserId == meetingUser.UserId);
        meetingUserDetails.User.MeetingRequests.First(x => x.Status == MeetingRequestStatusTypes.Found).Status = MeetingRequestStatusTypes.Aborted;

        if (meeting.Users.Count == 2)
        {
          var anotherUser = meeting.Users.First(x => x.UserId != meetingUser.UserId);
          anotherUser.User.MeetingRequests.First(x => x.Status == MeetingRequestStatusTypes.Found).Status = MeetingRequestStatusTypes.Searching;
          anotherUser.Status = MeetingUserStatusTypes.Left;
          meeting.Status = MeetingStatusTypes.Aborted;
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
