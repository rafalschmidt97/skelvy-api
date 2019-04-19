using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQueryHandler : QueryHandler<FindMeetingChatMessagesQuery, IList<MeetingChatMessageDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindMeetingChatMessagesQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public override async Task<IList<MeetingChatMessageDto>> Handle(FindMeetingChatMessagesQuery request)
    {
      var meetingUser = await _context.MeetingUsers
          .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.IsRemoved);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      const int pageSize = 20;
      var skip = (request.Page - 1) * pageSize;

      var messages = await _context.MeetingChatMessages
        .OrderByDescending(x => x.Date)
        .Skip(skip)

        .Where(x => x.MeetingId == meetingUser.MeetingId)
        .ProjectTo<MeetingChatMessageDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }
  }
}
