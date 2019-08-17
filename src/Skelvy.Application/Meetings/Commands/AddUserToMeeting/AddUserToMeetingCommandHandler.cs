using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UserJoinedGroup;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Meetings.Commands.AddUserToMeeting
{
  public class AddUserToMeetingCommandHandler : CommandHandler<AddUserToMeetingCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IMediator _mediator;

    public AddUserToMeetingCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IRelationsRepository relationsRepository,
      IMediator mediator)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _relationsRepository = relationsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(AddUserToMeetingCommand request)
    {
      var (meeting, groupUser) = await ValidateData(request);

      var addedGroupUser = new GroupUser(meeting.GroupId, request.UserId, groupUser.GetInheritedRole());
      await _groupUsersRepository.Add(addedGroupUser);
      await _mediator.Publish(new UserJoinedGroupEvent(addedGroupUser.UserId, addedGroupUser.GroupId));

      return Unit.Value;
    }

    private async Task<(Meeting, GroupUser)> ValidateData(AddUserToMeetingCommand request)
    {
      var existsUser = await _usersRepository.ExistsOne(request.UserId);

      if (!existsUser)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var existsAddedUser = await _usersRepository.ExistsOne(request.AddedUserId);

      if (!existsAddedUser)
      {
        throw new NotFoundException(nameof(User), request.AddedUserId);
      }

      var existsFriendRelation = await _relationsRepository
        .ExistsByUserIdAndRelatedUserIdAndType(request.UserId, request.AddedUserId, RelationType.Friend);

      if (!existsFriendRelation)
      {
        throw new NotFoundException(nameof(Relation), request.AddedUserId);
      }

      var meeting = await _meetingsRepository.FindOneNonFullByMeetingId(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var groupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}) not found.");
      }

      if (!groupUser.CanAddUserToGroup)
      {
        throw new ForbiddenException($"Entity {nameof(GroupUser)}(Id = {groupUser.Id}) does not have permission to add users");
      }

      var existsGroupAddedUser = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.AddedUserId, meeting.GroupId);

      if (existsGroupAddedUser)
      {
        throw new ConflictException($"Entity {nameof(GroupUser)}(UserId = {request.AddedUserId}, GroupId = {meeting.GroupId}) is already added.");
      }

      return (meeting, groupUser);
    }
  }
}
