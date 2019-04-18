using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommandHandler : CommandHandler<LeaveMeetingCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public LeaveMeetingCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(LeaveMeetingCommand request)
    {
      var meetingUser = await _context.MeetingUsers
        .Include(x => x.Meeting)
        .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Status == MeetingUserStatusTypes.Joined);

      if (meetingUser == null)
      {
        throw new NotFoundException(nameof(MeetingUser), request.UserId);
      }

      var meetingUsers = await _context.MeetingUsers
        .Include(x => x.MeetingRequest)
        .Where(x => x.MeetingId == meetingUser.MeetingId && x.Status == MeetingUserStatusTypes.Joined)
        .ToListAsync();

      meetingUser.Status = MeetingUserStatusTypes.Left;

      var meetingUserRequest = await _context.MeetingRequests
        .FirstOrDefaultAsync(x => x.Id == meetingUser.MeetingRequestId);

      meetingUserRequest.Status = MeetingRequestStatusTypes.Aborted;

      if (meetingUsers.Count == 2)
      {
        var anotherUser = meetingUsers.First(x => x.UserId != meetingUser.UserId);
        anotherUser.MeetingRequest.Status = MeetingRequestStatusTypes.Searching;
        anotherUser.Status = MeetingUserStatusTypes.Left;
        meetingUser.Meeting.Status = MeetingStatusTypes.Aborted;
      }

      await _context.SaveChangesAsync();
      await BroadcastUserLeftMeeting(meetingUser, meetingUsers);

      return Unit.Value;
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
