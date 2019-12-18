using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitationsDetails
{
  public class FindMeetingInvitationsDetailsQueryHandler : QueryHandler<FindMeetingInvitationsDetailsQuery, IList<MeetingInvitationDto>>
  {
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMapper _mapper;

    public FindMeetingInvitationsDetailsQueryHandler(
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMapper mapper)
    {
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _groupUsersRepository = groupUsersRepository;
      _meetingsRepository = meetingsRepository;
      _mapper = mapper;
    }

    public override async Task<IList<MeetingInvitationDto>> Handle(FindMeetingInvitationsDetailsQuery request)
    {
      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.UserId);
      }

      var groupUser = await _groupUsersRepository.FindOneWithGroupByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException(nameof(GroupUser), request.UserId);
      }

      var meetingInvitations = await _meetingInvitationsRepository.FindAllWithInvitedUserDetailsByMeetingId(request.MeetingId);
      return _mapper.Map<IList<MeetingInvitationDto>>(meetingInvitations);
    }
  }
}
