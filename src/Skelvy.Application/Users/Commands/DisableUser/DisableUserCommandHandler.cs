using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Commands;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommandHandler : CommandHandler<DisableUserCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public DisableUserCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(DisableUserCommand request)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsRemoved);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      if (user.IsDisabled)
      {
        throw new ConflictException($"Entity {nameof(User)}(Id = {request.Id}) is already disabled.");
      }

      await LeaveMeetings(user);
      user.Disable(request.Reason);

      await _context.SaveChangesAsync();
      await _notifications.BroadcastUserDisabled(user, request.Reason);

      return Unit.Value;
    }

    private async Task LeaveMeetings(User user) // Same logic as LeaveMeetingCommand
    {
      var meetingUser = await _context.MeetingUsers
        .Include(x => x.Meeting)
        .FirstOrDefaultAsync(x => x.UserId == user.Id && !x.IsRemoved);

      if (meetingUser != null)
      {
        var meetingUsers = await _context.MeetingUsers
          .Include(x => x.MeetingRequest)
          .Where(x => x.MeetingId == meetingUser.MeetingId && !x.IsRemoved)
          .ToListAsync();

        var userDetails = meetingUsers.First(x => x.UserId == meetingUser.UserId);

        userDetails.Remove(MeetingUserRemovedReasonTypes.Aborted);
        userDetails.MeetingRequest.Remove(MeetingRequestRemovedReasonTypes.Aborted);

        if (meetingUsers.Count == 2)
        {
          var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

          anotherUserDetails.Remove(MeetingUserRemovedReasonTypes.Expired);
          anotherUserDetails.MeetingRequest.UpdateStatus(MeetingRequestStatusTypes.Searching);
          meetingUser.Meeting.Remove(MeetingRemovedReasonTypes.Aborted);

          await _context.SaveChangesAsync();
          await BroadcastUserLeftMeeting(meetingUser, meetingUsers);
        }

        await _context.SaveChangesAsync();
        await BroadcastUserLeftMeeting(meetingUser, meetingUsers);
      }
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
