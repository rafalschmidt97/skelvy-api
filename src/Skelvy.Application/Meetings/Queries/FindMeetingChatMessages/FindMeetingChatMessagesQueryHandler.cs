using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQueryHandler : QueryHandler<FindMeetingChatMessagesQuery, IList<MeetingChatMessageDto>>
  {
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingChatMessagesRepository _chatMessagesRepository;
    private readonly IMapper _mapper;

    public FindMeetingChatMessagesQueryHandler(
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingChatMessagesRepository chatMessagesRepository,
      IMapper mapper)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _chatMessagesRepository = chatMessagesRepository;
      _mapper = mapper;
    }

    public override async Task<IList<MeetingChatMessageDto>> Handle(FindMeetingChatMessagesQuery request)
    {
      var meetingUser = await _meetingUsersRepository.FindOneByUserId(request.UserId);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      var messages = await _chatMessagesRepository.FindPageByMeetingId(meetingUser.MeetingId, request.Page);
      return _mapper.Map<IList<MeetingChatMessageDto>>(messages);
    }
  }
}
