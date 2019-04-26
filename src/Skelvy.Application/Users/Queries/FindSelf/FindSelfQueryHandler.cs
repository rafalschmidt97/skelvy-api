using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Users.Queries.FindSelf
{
  public class FindSelfQueryHandler : QueryHandler<FindSelfQuery, SelfModel>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingChatMessagesRepository _chatMessagesRepository;
    private readonly IMapper _mapper;

    public FindSelfQueryHandler(
      IUsersRepository usersRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingChatMessagesRepository chatMessagesRepository,
      IMapper mapper)
    {
      _usersRepository = usersRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingsRepository = meetingsRepository;
      _chatMessagesRepository = chatMessagesRepository;
      _mapper = mapper;
    }

    public override async Task<SelfModel> Handle(FindSelfQuery request)
    {
      var user = await _usersRepository.FindOneWithDetails(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meetingRequest = await _meetingRequestsRepository.FindOneWithDrinksByUserId(request.UserId);

      if (meetingRequest != null)
      {
        if (meetingRequest.IsFound)
        {
          var meeting = await _meetingsRepository.FindOneWithUsersDetailsAndDrinkByUserId(request.UserId);

          if (meeting == null)
          {
            throw new InternalServerErrorException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) not found " +
                                        $"while {nameof(MeetingRequest)} is marked as '{MeetingRequestStatusTypes.Found}'");
          }

          var messages = await _chatMessagesRepository.FindPageByMeetingId(meeting.Id);

          return new SelfModel(
            _mapper.Map<UserDto>(user),
            new MeetingModel(
              MeetingRequestStatusTypes.Found,
              _mapper.Map<MeetingDto>(meeting),
              _mapper.Map<IList<MeetingChatMessageDto>>(messages),
              _mapper.Map<MeetingRequestDto>(meetingRequest)));
        }

        return new SelfModel(
          _mapper.Map<UserDto>(user),
          new MeetingModel(
            MeetingRequestStatusTypes.Searching,
            _mapper.Map<MeetingRequestDto>(meetingRequest)));
      }

      return new SelfModel(_mapper.Map<UserDto>(user));
    }
  }
}
