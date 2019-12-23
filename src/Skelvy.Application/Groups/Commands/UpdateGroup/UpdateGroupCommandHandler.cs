using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Events.GroupUpdated;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Groups.Commands.UpdateGroup
{
  public class UpdateGroupCommandHandler : CommandHandler<UpdateGroupCommand>
  {
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;

    public UpdateGroupCommandHandler(IGroupsRepository groupsRepository, IGroupUsersRepository groupUsersRepository, IMediator mediator)
    {
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(UpdateGroupCommand request)
    {
      var group = await ValidateData(request);

      group.Update(request.Name);
      await _groupsRepository.Update(group);
      await _mediator.Publish(new GroupUpdatedEvent(request.UserId, group.Id));

      return Unit.Value;
    }

    private async Task<Group> ValidateData(UpdateGroupCommand request)
    {
      var groupUser = await _groupUsersRepository.FindOneWithGroupByUserIdAndGroupId(request.UserId, request.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException(nameof(GroupUser), request.UserId);
      }

      if (!groupUser.CanUpdateGroup())
      {
        throw new ForbiddenException($"{nameof(GroupUser)}({groupUser.Id}) does not have permission to update group.");
      }

      return groupUser.Group;
    }
  }
}
