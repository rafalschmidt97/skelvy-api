using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetingRequests
{
  public class FindMeetingRequestsQueryHandler : QueryHandler<FindMeetingRequestsQuery, IList<MeetingRequestDto>>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingMapper _mapper;

    public FindMeetingRequestsQueryHandler(
      IUsersRepository usersRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingMapper mapper)
    {
      _usersRepository = usersRepository;
      _requestsRepository = requestsRepository;
      _mapper = mapper;
    }

    public override async Task<IList<MeetingRequestDto>> Handle(FindMeetingRequestsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var requests = await _requestsRepository.FindAllSearchingWithActivitiesByUserId(request.UserId);
      return await _mapper.Map(requests, request.Language);
    }
  }
}
