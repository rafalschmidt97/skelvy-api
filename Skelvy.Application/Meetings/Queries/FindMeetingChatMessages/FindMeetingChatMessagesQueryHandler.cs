using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQueryHandler
    : IRequestHandler<FindMeetingChatMessagesQuery, ICollection<MeetingChatMessageDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;
    private readonly INotificationsService _notifications;

    public FindMeetingChatMessagesQueryHandler(SkelvyContext context, IMapper mapper, INotificationsService notifications)
    {
      _context = context;
      _mapper = mapper;
      _notifications = notifications;
    }

    public async Task<ICollection<MeetingChatMessageDto>> Handle(
      FindMeetingChatMessagesQuery request,
      CancellationToken cancellationToken)
    {
      var meetingUser =
        await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      var messages = await _context.MeetingChatMessages
        .Where(x => x.MeetingId == meetingUser.MeetingId && x.Date >= request.FromDate && x.Date <= request.ToDate)
        .ProjectTo<MeetingChatMessageDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

      await _notifications.BroadcastMessages(messages, meetingUser.UserId, cancellationToken);
      return messages;
    }
  }
}
