using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetingSuggestions
{
  public class FindMeetingSuggestionsQueryHandler : QueryHandler<FindMeetingSuggestionsQuery, MeetingSuggestionsModel>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMapper _mapper;

    public FindMeetingSuggestionsQueryHandler(
      IUsersRepository usersRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingsRepository meetingsRepository,
      IMapper mapper)
    {
      _usersRepository = usersRepository;
      _requestsRepository = requestsRepository;
      _meetingsRepository = meetingsRepository;
      _mapper = mapper;
    }

    public override async Task<MeetingSuggestionsModel> Handle(FindMeetingSuggestionsQuery request)
    {
      await ValidateData(request);

      var requests = await _requestsRepository.FindAllCloseToPreferences(request.UserId, request.Latitude, request.Longitude);
      var meetings = await _meetingsRepository.FindAllCloseToPreferences(request.UserId, request.Latitude, request.Longitude);

      return new MeetingSuggestionsModel(
        _mapper.Map<IList<MeetingRequestDto>>(requests),
        _mapper.Map<IList<MeetingDto>>(meetings));
    }

    private async Task ValidateData(FindMeetingSuggestionsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var requestExists = await _requestsRepository.ExistsOneByUserId(request.UserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }
    }
  }
}
