using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Queries.FindSelf
{
  public class FindSelfQueryHandler : QueryHandler<FindSelfQuery, SelfViewModel>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindSelfQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public override async Task<SelfViewModel> Handle(FindSelfQuery request)
    {
      var user = await FindUser(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meetingRequest = await FindMeetingRequest(request.UserId);

      if (meetingRequest != null)
      {
        var meeting = await FindMeeting(request.UserId);

        if (meeting != null)
        {
          var messages = await FindMeetingChatMessages(meeting.Id);

          return new SelfViewModel(
            _mapper.Map<UserDto>(user),
            new MeetingViewModel(
              MeetingRequestStatusTypes.Found,
              _mapper.Map<MeetingDto>(meeting),
              _mapper.Map<IList<MeetingChatMessageDto>>(messages),
              _mapper.Map<MeetingRequestDto>(meetingRequest)));
        }

        return new SelfViewModel(
          _mapper.Map<UserDto>(user),
          new MeetingViewModel(
            MeetingRequestStatusTypes.Searching,
            _mapper.Map<MeetingRequestDto>(meetingRequest)));
      }

      return new SelfViewModel(_mapper.Map<UserDto>(user));
    }

    private async Task<User> FindUser(int userId)
    {
      return await _context.Users
        .Include(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .FirstOrDefaultAsync(x => x.Id == userId);
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
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == userId && !y.IsRemoved));
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
