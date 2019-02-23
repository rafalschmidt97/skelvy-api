using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.DeleteExpiredMeetingsAndMeetingRequests
{
  public class DeleteExpiredMeetingsAndMeetingRequestsCommandHandler
    : IRequestHandler<DeleteExpiredMeetingsAndMeetingRequestsCommand>
  {
    private readonly SkelvyContext _context;

    public DeleteExpiredMeetingsAndMeetingRequestsCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(
      DeleteExpiredMeetingsAndMeetingRequestsCommand request,
      CancellationToken cancellationToken)
    {
      var today = DateTime.Now.Date;
      var requestsToDelete = await _context.MeetingRequests.Where(x => x.MaxDate < today).ToListAsync(cancellationToken);
      var meetingsToDelete = await _context.Meetings.Where(x => x.Date < today).ToListAsync(cancellationToken);
      var isDataChanged = false;

      if (requestsToDelete.Count != 0)
      {
        _context.MeetingRequests.RemoveRange(requestsToDelete);
        isDataChanged = true;
      }

      if (meetingsToDelete.Count != 0)
      {
        _context.Meetings.RemoveRange(meetingsToDelete);
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
