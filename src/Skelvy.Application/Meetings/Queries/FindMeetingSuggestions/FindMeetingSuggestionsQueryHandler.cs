using System.Threading.Tasks;
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
    private readonly IMeetingMapper _mapper;

    public FindMeetingSuggestionsQueryHandler(
      IUsersRepository usersRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingMapper mapper)
    {
      _usersRepository = usersRepository;
      _requestsRepository = requestsRepository;
      _meetingsRepository = meetingsRepository;
      _mapper = mapper;
    }

    public override async Task<MeetingSuggestionsModel> Handle(FindMeetingSuggestionsQuery request)
    {
      await ValidateData(request);

      var requests = await _requestsRepository.FindAllCloseToPreferencesWithUserDetails(request.UserId, request.Latitude, request.Longitude);
      var meetings = await _meetingsRepository.FindAllCloseToPreferencesWithUsersDetails(request.UserId, request.Latitude, request.Longitude);

      return await _mapper.Map(requests, meetings, request.Language);
    }

    private async Task ValidateData(FindMeetingSuggestionsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var foundRequestExists = await _requestsRepository.ExistsOneFoundByUserId(request.UserId);

      if (foundRequestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}(UserId={request.UserId}) is marked as found. Leave meeting first.");
      }
    }
  }
}
