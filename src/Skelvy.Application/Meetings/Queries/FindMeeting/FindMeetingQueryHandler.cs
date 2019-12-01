using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryHandler : QueryHandler<FindMeetingQuery, MeetingDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingMapper _meetingMapper;

    public FindMeetingQueryHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingMapper meetingMapper)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingMapper = meetingMapper;
    }

    public override async Task<MeetingDto> Handle(FindMeetingQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meeting = await _meetingsRepository.FindOneWithActivityByMeetingIdAndUserId(request.MeetingId, request.UserId);
      return await _meetingMapper.Map(meeting, request.Language);
    }
  }
}
