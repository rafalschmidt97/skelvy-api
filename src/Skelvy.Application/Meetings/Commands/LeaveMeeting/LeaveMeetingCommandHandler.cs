using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Infrastructure.Notifications;
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
      var user = await _context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Status == MeetingUserStatusTypes.Joined);

      if (user == null)
      {
        throw new NotFoundException(nameof(MeetingUser), request.UserId);
      }

      var meeting = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.MeetingRequests)
        .FirstOrDefaultAsync(x => x.Id == user.MeetingId);

      var meetingUser = meeting.Users.First(x => x.UserId == user.UserId);
      meetingUser.Status = MeetingUserStatusTypes.Left;
      meetingUser.User.MeetingRequests.First(x => x.Status == MeetingRequestStatusTypes.Found).Status = MeetingRequestStatusTypes.Aborted;

      if (meeting.Users.Count == 2)
      {
        var anotherUser = meeting.Users.First(x => x.UserId != user.UserId);
        anotherUser.User.MeetingRequests.First(x => x.Status == MeetingRequestStatusTypes.Found).Status = MeetingRequestStatusTypes.Searching;
        anotherUser.Status = MeetingUserStatusTypes.Left;
        meeting.Status = MeetingStatusTypes.Aborted;
      }

      await _context.SaveChangesAsync();
      await BroadcastUserLeftMeeting(meetingUser, meeting.Users);
      return Unit.Value;
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
