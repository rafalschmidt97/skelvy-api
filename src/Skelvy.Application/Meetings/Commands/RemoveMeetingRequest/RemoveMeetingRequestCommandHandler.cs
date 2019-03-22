using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommandHandler : IRequestHandler<RemoveMeetingRequestCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveMeetingRequestCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(RemoveMeetingRequestCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (user != null)
      {
        throw new ConflictException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) exists. Leave meeting instead.");
      }

      var meetingRequest = await _context.MeetingRequests
        .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) not found.");
      }

      _context.MeetingRequests.Remove(meetingRequest);

      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}
