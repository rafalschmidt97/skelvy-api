using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryHandler : QueryHandler<FindMeetingQuery, MeetingModel>
  {
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingChatMessagesRepository _messagesRepository;
    private readonly IMapper _mapper;

    public FindMeetingQueryHandler(
      IMeetingRequestsRepository requestsRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingChatMessagesRepository messagesRepository,
      IMapper mapper)
    {
      _requestsRepository = requestsRepository;
      _meetingsRepository = meetingsRepository;
      _messagesRepository = messagesRepository;
      _mapper = mapper;
    }

    public override async Task<MeetingModel> Handle(FindMeetingQuery request)
    {
      var meetingRequest = await _requestsRepository.FindOneWithDrinkTypesByUserId(request.UserId);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) not found.");
      }

      if (meetingRequest.IsFound)
      {
        var meeting = await _meetingsRepository.FindOneWithUsersDetailsAndDrinkByUserId(request.UserId);

        if (meeting == null)
        {
          throw new InternalServerErrorException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) not found " +
                                                 $"while {nameof(MeetingRequest)} is marked as '{MeetingRequestStatusTypes.Found}'");
        }

        var messages = await _messagesRepository.FindPageByMeetingId(meeting.Id);

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
  }
}
