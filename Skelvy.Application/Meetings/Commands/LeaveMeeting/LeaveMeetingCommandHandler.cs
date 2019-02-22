using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommandHandler : IRequestHandler<LeaveMeetingCommand>
  {
    private readonly SkelvyContext _context;

    public LeaveMeetingCommandHandler(SkelvyContext context)
    {
      _context = context;
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

      var userDetails = meeting.Users.First(x => x.UserId == user.UserId);
      _context.MeetingUsers.Remove(userDetails);
      _context.MeetingRequests.Remove(userDetails.User.MeetingRequest);

      if (meeting.Users.Count == 2)
      {
        meeting.Users.First(x => x.UserId != user.UserId).User.MeetingRequest.Status = MeetingStatusTypes.Searching;
        _context.Meetings.Remove(meeting);
      }

      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}
