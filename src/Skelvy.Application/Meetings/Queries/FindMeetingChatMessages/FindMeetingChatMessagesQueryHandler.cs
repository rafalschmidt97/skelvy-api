using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQueryHandler
    : IRequestHandler<FindMeetingChatMessagesQuery, IList<MeetingChatMessageDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindMeetingChatMessagesQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IList<MeetingChatMessageDto>> Handle(
      FindMeetingChatMessagesQuery request,
      CancellationToken cancellationToken)
    {
      var meetingUser =
        await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      return await _context.MeetingChatMessages
        .Where(x => x.MeetingId == meetingUser.MeetingId && x.Date >= request.FromDate && x.Date <= request.ToDate)
        .ProjectTo<MeetingChatMessageDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
  }
}
