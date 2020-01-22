using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindUsersToInviteToMeeting
{
  public class FindUsersToInviteToMeetingQueryHandler : QueryHandler<FindUsersToInviteToMeetingQuery, IList<UserDto>>
  {
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IMapper _mapper;

    public FindUsersToInviteToMeetingQueryHandler(
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingsRepository meetingsRepository,
      IRelationsRepository relationsRepository,
      IMapper mapper)
    {
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _groupUsersRepository = groupUsersRepository;
      _meetingsRepository = meetingsRepository;
      _relationsRepository = relationsRepository;
      _mapper = mapper;
    }

    public override async Task<IList<UserDto>> Handle(FindUsersToInviteToMeetingQuery request)
    {
      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.UserId);
      }

      var groupUserExists = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (!groupUserExists)
      {
        throw new NotFoundException(nameof(GroupUser), request.UserId);
      }

      var meetingInvitations = await _meetingInvitationsRepository.FindAllWithInvitedUserDetailsByMeetingId(request.MeetingId);
      var invitationsUserId = meetingInvitations.Select(x => x.InvitedUserId).ToList();
      var meetingUsers = await _groupUsersRepository.FindAllByGroupId(meeting.GroupId);
      var groupUserId = meetingUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();
      var usersToExclude = invitationsUserId.Concat(groupUserId).ToList();

      var usersToInvite = await _relationsRepository
        .FindPageUsersWithRelatedDetailsByUserIdAndTypeExcludeList(request.UserId, RelationType.Friend, usersToExclude, request.Page);

      return _mapper.Map<IList<UserDto>>(usersToInvite);
    }
  }
}
