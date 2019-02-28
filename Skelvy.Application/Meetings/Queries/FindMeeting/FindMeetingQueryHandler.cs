using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryHandler : IRequestHandler<FindMeetingQuery, MeetingViewModel>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindMeetingQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<MeetingViewModel> Handle(FindMeetingQuery request, CancellationToken cancellationToken)
    {
      var meetingRequest = await _context.MeetingRequests
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) not found.");
      }

      var meeting = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(y => y.User)
        .ThenInclude(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .Include(x => x.Drink)
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == request.UserId), cancellationToken);

      if (meeting != null)
      {
        return new MeetingViewModel
        {
          Status = MeetingStatusTypes.Found,
          Meeting = _mapper.Map<MeetingDto>(meeting),
          Request = _mapper.Map<MeetingRequestDto>(meetingRequest)
        };
      }

      return new MeetingViewModel
      {
        Status = MeetingStatusTypes.Searching,
        Request = _mapper.Map<MeetingRequestDto>(meetingRequest)
      };
    }
  }
}
