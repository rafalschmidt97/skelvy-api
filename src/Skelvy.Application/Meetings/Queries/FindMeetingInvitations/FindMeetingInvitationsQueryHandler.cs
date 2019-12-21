using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitations
{
  public class FindMeetingInvitationsQueryHandler : QueryHandler<FindMeetingInvitationsQuery, IList<SelfMeetingInvitationDto>>
  {
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingMapper _meetingMapper;

    public FindMeetingInvitationsQueryHandler(
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IUsersRepository usersRepository,
      IMeetingMapper meetingMapper)
    {
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _usersRepository = usersRepository;
      _meetingMapper = meetingMapper;
    }

    public override async Task<IList<SelfMeetingInvitationDto>> Handle(FindMeetingInvitationsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meetingInvitations = await _meetingInvitationsRepository.FindAllWithActivityAndUsersDetailsByUserId(request.UserId);
      return await _meetingMapper.Map(meetingInvitations, request.Language);
    }
  }
}
