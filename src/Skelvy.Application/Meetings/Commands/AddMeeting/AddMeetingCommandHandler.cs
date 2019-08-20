using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Activities.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Commands.AddMeeting
{
  public class AddMeetingCommandHandler : CommandHandlerData<AddMeetingCommand, MeetingDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IActivitiesRepository _activitiesRepository;
    private readonly IMapper _mapper;

    public AddMeetingCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IActivitiesRepository activitiesRepository,
      IMapper mapper)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _activitiesRepository = activitiesRepository;
      _mapper = mapper;
    }

    public override async Task<MeetingDto> Handle(AddMeetingCommand request)
    {
      await ValidateData(request);

      using (var transaction = _groupUsersRepository.BeginTransaction())
      {
        var group = new Group();
        await _groupsRepository.Add(group);

        var meeting = new Meeting(
          request.Date,
          request.Latitude,
          request.Longitude,
          request.Size,
          true,
          true,
          group.Id,
          request.ActivityId);

        await _meetingsRepository.Add(meeting);

        var groupUser = new GroupUser(meeting.GroupId, request.UserId, GroupUserRoleType.Owner);
        await _groupUsersRepository.Add(groupUser);

        transaction.Commit();

        return _mapper.Map<MeetingDto>(meeting);
      }
    }

    private async Task ValidateData(AddMeetingCommand request)
    {
      var existsUser = await _usersRepository.ExistsOne(request.UserId);

      if (!existsUser)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var existsActivity = await _activitiesRepository.ExistsOne(request.ActivityId);

      if (!existsActivity)
      {
        throw new NotFoundException(nameof(Activity), request.ActivityId);
      }

      var ownMeetingsCount = await _meetingsRepository.CountOwnMeetingsByUserId(request.UserId);

      if (ownMeetingsCount >= 3)
      {
        throw new ConflictException($"{nameof(User)}(Id = {request.UserId}) has already {ownMeetingsCount} {nameof(Meeting)}s. " +
                                    $"You can have up to 3 {nameof(Meeting)} simultaneity. Remove one first.");
      }
    }
  }
}
