using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMessages
{
  public class FindMessagesQueryHandler : QueryHandler<FindMessagesQuery, IList<MessageDto>>
  {
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IMapper _mapper;

    public FindMessagesQueryHandler(
      IGroupUsersRepository groupUsersRepository,
      IMessagesRepository messagesRepository,
      IMapper mapper)
    {
      _groupUsersRepository = groupUsersRepository;
      _messagesRepository = messagesRepository;
      _mapper = mapper;
    }

    public override async Task<IList<MessageDto>> Handle(FindMessagesQuery request)
    {
      var meetingUser = await _groupUsersRepository.FindOneByUserId(request.UserId);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}) not found.");
      }

      var messagesBefore = await _messagesRepository.FindPageBeforeByMeetingId(meetingUser.GroupId, request.BeforeDate);
      return _mapper.Map<IList<MessageDto>>(messagesBefore);
    }
  }
}
