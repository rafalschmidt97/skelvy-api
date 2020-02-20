using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Events.GroupUserRoleUpdated;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Groups.Commands.UpdateGroupUserRole
{
  public class UpdateGroupUserRoleCommandHandler : CommandHandler<UpdateGroupUserRoleCommand>
  {
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;

    public UpdateGroupUserRoleCommandHandler(
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator)
    {
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(UpdateGroupUserRoleCommand request)
    {
      var groupUser = await ValidateData(request);

      groupUser.UpdateRole(request.Role);
      await _groupUsersRepository.Update(groupUser);
      await _mediator.Publish(
        new GroupUserRoleUpdatedEvent(
          request.UserId,
          request.GroupId,
          request.UpdatedUserId,
          request.Role));

      return Unit.Value;
    }

    private async Task<GroupUser> ValidateData(UpdateGroupUserRoleCommand request)
    {
      var group = await _groupsRepository.FindOne(request.GroupId);

      if (group == null)
      {
        throw new NotFoundException(nameof(Group), request.GroupId);
      }

      var groupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UserId, group.Id);

      if (groupUser == null)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {group.Id}) not found.");
      }

      var removedGroupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UpdatedUserId, group.Id);

      if (removedGroupUser == null)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {group.Id}) not found.");
      }

      if (!groupUser.CanUpdateRole(removedGroupUser, request.Role))
      {
        throw new ForbiddenException($"{nameof(GroupUser)}({groupUser.Id}) does not have permission to update user role.");
      }

      return removedGroupUser;
    }
  }
}
