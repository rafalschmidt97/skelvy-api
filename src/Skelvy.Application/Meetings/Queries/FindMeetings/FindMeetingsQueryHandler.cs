using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries.FindMeetings
{
  public class FindMeetingsQueryHandler : QueryHandler<FindMeetingsQuery, MeetingModel>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IMeetingMapper _meetingMapper;
    private readonly IMapper _mapper;

    public FindMeetingsQueryHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IGroupsRepository groupsRepository,
      IMeetingMapper meetingMapper,
      IMapper mapper)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _groupsRepository = groupsRepository;
      _meetingMapper = meetingMapper;
      _mapper = mapper;
    }

    public override async Task<MeetingModel> Handle(FindMeetingsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meetings = await _meetingsRepository.FindAllWithActivityByUserId(request.UserId);
      var groups = await _groupsRepository.FindAllWithUsersDetailsAndMessagesByUserId(request.UserId);

      return new MeetingModel(
        await _meetingMapper.Map(meetings, request.Language),
        _mapper.Map<IList<GroupDto>>(groups));
    }
  }
}
