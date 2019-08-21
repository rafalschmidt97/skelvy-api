using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitations
{
  public class FindMeetingInvitationsQueryHandler : QueryHandler<FindMeetingInvitationsQuery, IList<MeetingInvitationDto>>
  {
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public FindMeetingInvitationsQueryHandler(
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IUsersRepository usersRepository,
      IMapper mapper)
    {
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _usersRepository = usersRepository;
      _mapper = mapper;
    }

    public override async Task<IList<MeetingInvitationDto>> Handle(FindMeetingInvitationsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var friendRequests = await _meetingInvitationsRepository.FindAllWithInvitingDetailsByUserId(request.UserId);
      return _mapper.Map<IList<MeetingInvitationDto>>(friendRequests);
    }
  }
}