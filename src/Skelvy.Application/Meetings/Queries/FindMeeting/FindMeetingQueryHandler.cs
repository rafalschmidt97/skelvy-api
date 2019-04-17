using System.Collections.Generic;
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
      var meetingRequest = await FindMeetingRequest(request.UserId);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) not found.");
      }

      var meeting = await FindMeeting(request.UserId);

      if (meeting != null)
      {
        var messages = await FindMeetingChatMessages(meeting.Id);

        return new MeetingViewModel
        {
          Status = MeetingRequestStatusTypes.Found,
          Meeting = _mapper.Map<MeetingDto>(meeting),
          MeetingMessages = _mapper.Map<IList<MeetingChatMessageDto>>(messages),
          Request = _mapper.Map<MeetingRequestDto>(meetingRequest),
        };
      }

      return new MeetingViewModel
      {
        Status = MeetingRequestStatusTypes.Searching,
        Request = _mapper.Map<MeetingRequestDto>(meetingRequest),
      };
    }

    private async Task<MeetingRequest> FindMeetingRequest(int userId)
    {
      return await _context.MeetingRequests
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .FirstOrDefaultAsync(x => x.UserId == userId &&
                                  (x.Status == MeetingRequestStatusTypes.Searching || x.Status == MeetingRequestStatusTypes.Found));
    }

    private async Task<Meeting> FindMeeting(int userId)
    {
      return await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(y => y.User)
        .ThenInclude(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .Include(x => x.Drink)
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == userId && y.Status == MeetingUserStatusTypes.Joined));
    }

    private async Task<List<MeetingChatMessage>> FindMeetingChatMessages(int meetingId)
    {
      const int pageSize = 20;

      return await _context.MeetingChatMessages
        .OrderByDescending(p => p.Date)
        .Take(pageSize)
        .Where(x => x.MeetingId == meetingId)
        .ToListAsync();
    }
  }
}
