using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Relations.Queries;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries.Sync
{
  public class SyncQueryHandler : QueryHandler<SyncQuery, SyncModel>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IMeetingMapper _meetingMapper;
    private readonly IMapper _mapper;

    public SyncQueryHandler(
      IUsersRepository usersRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingsRepository meetingsRepository,
      IGroupsRepository groupsRepository,
      IFriendInvitationsRepository friendInvitationsRepository,
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IMeetingMapper meetingMapper,
      IMapper mapper)
    {
      _usersRepository = usersRepository;
      _requestsRepository = requestsRepository;
      _meetingsRepository = meetingsRepository;
      _groupsRepository = groupsRepository;
      _friendInvitationsRepository = friendInvitationsRepository;
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _meetingMapper = meetingMapper;
      _mapper = mapper;
    }

    public override async Task<SyncModel> Handle(SyncQuery request)
    {
      var existsUser = await _usersRepository.ExistsOne(request.UserId);

      if (!existsUser)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var requests = await _requestsRepository.FindAllSearchingWithActivitiesByUserId(request.UserId);
      var meetings = await _meetingsRepository.FindAllWithActivityByUserId(request.UserId);
      var groups = await _groupsRepository.FindAllWithUsersDetailsAndMessagesByUserId(request.UserId);
      var friendInvitations = await _friendInvitationsRepository.FindAllWithInvitingDetailsByUserId(request.UserId);
      var meetingInvitations = await _meetingInvitationsRepository.FindAllWithActivityAndUsersDetailsByUserId(request.UserId);

      return new SyncModel(
        await _meetingMapper.Map(requests, request.Language),
        await _meetingMapper.Map(meetings, request.Language),
        _mapper.Map<IList<GroupDto>>(groups),
        _mapper.Map<IList<FriendInvitationsDto>>(friendInvitations),
        await _meetingMapper.Map(meetingInvitations, request.Language));
    }
  }
}
