using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommandHandler : CommandHandler<RemoveMeetingRequestCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveMeetingRequestCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public override async Task<Unit> Handle(RemoveMeetingRequestCommand request)
    {
      var meetingUser = await _context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.IsRemoved);

      if (meetingUser != null)
      {
        throw new ConflictException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) exists. Leave meeting instead.");
      }

      var meetingRequest = await _context.MeetingRequests
        .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Status == MeetingRequestStatusTypes.Searching && !x.IsRemoved);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) not found.");
      }

      meetingRequest.Abort();

      await _context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
