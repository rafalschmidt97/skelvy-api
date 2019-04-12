using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommandHandler : CommandHandler<RemoveUserCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveUserCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(RemoveUserCommand request)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      if (user.IsDeleted)
      {
        throw new ConflictException($"Entity {nameof(User)}(Id = {request.Id}) is already deleted.");
      }

      await LeaveMeetings(user);

      user.IsDeleted = true;
      user.DeletionDate = DateTimeOffset.UtcNow.AddMonths(3);

      await _context.SaveChangesAsync();
      await _notifications.BroadcastUserDeleted(user);

      return Unit.Value;
    }

    private async Task LeaveMeetings(User user)
    {
      var meetingUser = await _context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == MeetingUserStatusTypes.Joined);

      if (meetingUser != null)
      {
        var meeting = await _context.Meetings
          .Include(x => x.Users)
          .ThenInclude(x => x.User)
          .ThenInclude(x => x.MeetingRequests)
          .FirstOrDefaultAsync(x => x.Id == meetingUser.MeetingId && x.Status == MeetingStatusTypes.Active);

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

        await _context.SaveChangesAsync();
        await BroadcastUserLeftMeeting(meetingUser, meeting.Users);
      }
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
