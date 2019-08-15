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
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var requests = await _requestsRepository
        .FindAllCloseWithUserDetailsByUserIdAndLocation(request.UserId, request.Latitude, request.Longitude);

      var meetings = await _meetingsRepository
        .FindAllNonHiddenCloseWithUsersDetailsByUserIdAndLocation(request.UserId, request.Latitude, request.Longitude);

      return await _mapper.Map(requests, meetings, request.Language);
    }
  }
}
