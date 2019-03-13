using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings
{
  public class RemoveExpiredMeetingsCommandHandler
    : IRequestHandler<RemoveExpiredMeetingsCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveExpiredMeetingsCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(
      RemoveExpiredMeetingsCommand request,
      CancellationToken cancellationToken)
    {
      var today = DateTimeOffset.Now;
      var meetingsToRemove = await _context.Meetings.Where(x => x.Date < today.AddDays(-1)).ToListAsync(cancellationToken);
      var isDataChanged = false;

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
