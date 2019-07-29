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
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingChatMessagesRepository _chatMessagesRepository;
    private readonly IMapper _mapper;

    public FindMeetingChatMessagesQueryHandler(
      IGroupUsersRepository groupUsersRepository,
      IMeetingChatMessagesRepository chatMessagesRepository,
      IMapper mapper)
    {
      _groupUsersRepository = groupUsersRepository;
      _chatMessagesRepository = chatMessagesRepository;
      _mapper = mapper;
    }

    public override async Task<IList<MeetingChatMessageDto>> Handle(FindMeetingChatMessagesQuery request)
    {
      var meetingUser = await _groupUsersRepository.FindOneByUserId(request.UserId);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}) not found.");
      }

      var messagesBefore = await _chatMessagesRepository.FindPageBeforeByMeetingId(meetingUser.GroupId, request.BeforeDate);
      return _mapper.Map<IList<MeetingChatMessageDto>>(messagesBefore);
    }
  }
}
