using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryHandler : QueryHandler<FindMeetingQuery, MeetingViewModel>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindMeetingQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public override async Task<MeetingViewModel> Handle(FindMeetingQuery request)
    {
      var meetingRequest = await _context.MeetingRequests
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .FirstOrDefaultAsync(x => x.UserId == request.UserId && (x.Status == MeetingRequestStatusTypes.Searching || x.Status == MeetingRequestStatusTypes.Found));

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
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == request.UserId && y.Status == MeetingUserStatusTypes.Joined));

      if (meeting != null)
      {
        return new MeetingViewModel
        {
          Status = MeetingRequestStatusTypes.Found,
          Meeting = _mapper.Map<MeetingDto>(meeting),
          Request = _mapper.Map<MeetingRequestDto>(meetingRequest),
        };
      }

      return new MeetingViewModel
      {
        Status = MeetingRequestStatusTypes.Searching,
        Request = _mapper.Map<MeetingRequestDto>(meetingRequest),
      };
    }
  }
}
