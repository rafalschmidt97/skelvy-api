using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.DeleteEmptyMeetings
{
  public class DeleteEmptyMeetingsCommandHandler : IRequestHandler<DeleteEmptyMeetingsCommand>
  {
    private readonly SkelvyContext _context;

    public DeleteEmptyMeetingsCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteEmptyMeetingsCommand request, CancellationToken cancellationToken)
    {
      var meetings = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.MeetingRequest)
        .ToListAsync(cancellationToken);
      var isDataChanged = false;

      foreach (var meeting in meetings)
      {
        if (meeting.Users.Count <= 1)
        {
          DeleteMeeting(meeting);
          isDataChanged = true;
        }
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync(cancellationToken);
      }

      return Unit.Value;
    }

    private void DeleteMeeting(Meeting meeting)
    {
      if (meeting.Users.Count == 1)
      {
        meeting.Users.First().User.MeetingRequest.Status = MeetingStatusTypes.Searching;
      }

      _context.Meetings.Remove(meeting);
    }
  }
}
