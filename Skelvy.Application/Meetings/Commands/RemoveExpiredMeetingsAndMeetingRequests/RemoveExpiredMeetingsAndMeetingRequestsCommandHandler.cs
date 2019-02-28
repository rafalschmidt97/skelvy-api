using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingsAndMeetingRequests
{
  public class RemoveExpiredMeetingsAndMeetingRequestsCommandHandler
    : IRequestHandler<RemoveExpiredMeetingsAndMeetingRequestsCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveExpiredMeetingsAndMeetingRequestsCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(
      RemoveExpiredMeetingsAndMeetingRequestsCommand request,
      CancellationToken cancellationToken)
    {
      var today = DateTime.Now.Date;
      var requestsToRemove = await _context.MeetingRequests.Where(x => x.MaxDate < today).ToListAsync(cancellationToken);
      var meetingsToRemove = await _context.Meetings.Where(x => x.Date < today).ToListAsync(cancellationToken);
      var isDataChanged = false;

      if (requestsToRemove.Count != 0)
      {
        _context.MeetingRequests.RemoveRange(requestsToRemove);
        isDataChanged = true;
      }

      if (meetingsToRemove.Count != 0)
      {
        _context.Meetings.RemoveRange(meetingsToRemove);
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
