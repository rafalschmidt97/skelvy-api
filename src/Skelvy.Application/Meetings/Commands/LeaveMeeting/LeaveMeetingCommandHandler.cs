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
        .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.IsRemoved);

      if (meetingUser == null)
      {
        throw new NotFoundException(nameof(MeetingUser), request.UserId);
      }

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

      return Unit.Value;
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
