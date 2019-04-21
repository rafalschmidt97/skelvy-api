using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryHandler : QueryHandler<FindMeetingQuery, MeetingModel>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindMeetingQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public override async Task<MeetingModel> Handle(FindMeetingQuery request)
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

        return new MeetingModel(
          MeetingRequestStatusTypes.Found,
          _mapper.Map<MeetingDto>(meeting),
          _mapper.Map<IList<MeetingChatMessageDto>>(messages),
          _mapper.Map<MeetingRequestDto>(meetingRequest));
      }

      return new MeetingModel(
        MeetingRequestStatusTypes.Searching,
        _mapper.Map<MeetingRequestDto>(meetingRequest));
    }

    private async Task<MeetingRequest> FindMeetingRequest(int userId)
    {
      return await _context.MeetingRequests
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved);
    }

    private async Task<Meeting> FindMeeting(int userId)
    {
      return await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(y => y.User)
        .ThenInclude(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .Include(x => x.Drink)
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == userId && !x.IsRemoved));
    }

    private async Task<List<MeetingChatMessage>> FindMeetingChatMessages(int meetingId)
    {
      const int pageSize = 20;

      var messages = await _context.MeetingChatMessages
        .OrderByDescending(p => p.Date)
        .Take(pageSize)
        .Where(x => x.MeetingId == meetingId)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }
  }
}
