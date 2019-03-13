using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests
{
  public class RemoveExpiredMeetingRequestsCommandHandler
    : IRequestHandler<RemoveExpiredMeetingRequestsCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveExpiredMeetingRequestsCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(
      RemoveExpiredMeetingRequestsCommand request,
      CancellationToken cancellationToken)
    {
      var today = DateTimeOffset.Now;
      var requestsToRemove = await _context.MeetingRequests.Where(x => x.MaxDate < today).ToListAsync(cancellationToken);
      var isDataChanged = false;

      if (requestsToRemove.Count != 0)
      {
        _context.MeetingRequests.RemoveRange(requestsToRemove);
        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync(cancellationToken);
      }

      return Unit.Value;
    }
  }
}
