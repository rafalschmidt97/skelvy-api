using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Events.UserRemovedFromGroup;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Groups.Commands.RemoveUserFromGroup
{
  public class RemoveUserFromGroupCommandHandler : CommandHandler<RemoveUserFromGroupCommand>
  {
    private readonly IGroupsRepository _groupRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;

    public RemoveUserFromGroupCommandHandler(
      IGroupsRepository groupRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator)
    {
      _groupRepository = groupRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveUserFromGroupCommand request)
    {
      var (group, removedGroupUser) = await ValidateData(request);

      removedGroupUser.Remove();
      await _groupUsersRepository.Update(removedGroupUser);

      await _mediator.Publish(
        new UserRemovedFromGroupEvent(request.UserId, removedGroupUser.UserId, group.Id));

      return Unit.Value;
    }

    private async Task<(Group, GroupUser)> ValidateData(RemoveUserFromGroupCommand request)
    {
      var group = await _groupRepository.FindOne(request.GroupId);

      if (group == null)
      {
        throw new NotFoundException(nameof(Group), request.GroupId);
      }

      var groupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UserId, group.Id);

      if (groupUser == null)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {group.Id}) not found.");
      }

      var removedGroupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.RemovingUserId, group.Id);

      if (removedGroupUser == null)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {group.Id}) not found.");
      }

      if (!groupUser.CanRemoveUserFromGroup(removedGroupUser))
      {
        throw new ForbiddenException($"{nameof(GroupUser)}({groupUser.Id}) does not have permission to remove user");
      }

      return (group, removedGroupUser);
    }
  }
}
