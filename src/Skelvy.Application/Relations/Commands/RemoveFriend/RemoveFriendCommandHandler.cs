using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Relations.Commands.RemoveFriend
{
  public class RemoveFriendCommandHandler : CommandHandler<RemoveFriendCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IRelationsRepository _relationsRepository;

    public RemoveFriendCommandHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(RemoveFriendCommand request)
    {
      var friendRelations = await ValidateData(request);

      foreach (var relation in friendRelations)
      {
        relation.Abort();
      }

      await _relationsRepository.UpdateRange(friendRelations);

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
          $"Entity {nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.FriendUserId}) not found.");
      }

      return friendRelations;
    }
  }
}
