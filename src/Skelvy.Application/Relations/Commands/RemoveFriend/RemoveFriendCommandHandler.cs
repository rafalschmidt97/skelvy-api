using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Events.FriendRemoved;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Relations.Commands.RemoveFriend
{
  public class RemoveFriendCommandHandler : CommandHandler<RemoveFriendCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IMediator _mediator;

    public RemoveFriendCommandHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository,
      IMediator mediator)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveFriendCommand request)
    {
      var friendRelations = await ValidateData(request);

      foreach (var relation in friendRelations)
      {
        relation.Abort();
      }

      await _relationsRepository.UpdateRange(friendRelations);
      await _mediator.Publish(new FriendRemovedEvent(request.UserId, request.FriendUserId));

      return Unit.Value;
    }

    private async Task<IList<Relation>> ValidateData(RemoveFriendCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var friendRelations = await _relationsRepository
        .FindAllByUserIdAndRelatedUserIdAndTypeTwoWay(request.UserId, request.FriendUserId, RelationType.Friend);

      if (!friendRelations.Any())
      {
        throw new NotFoundException(
          $"{nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.FriendUserId}) not found.");
      }

      return friendRelations;
    }
  }
}
