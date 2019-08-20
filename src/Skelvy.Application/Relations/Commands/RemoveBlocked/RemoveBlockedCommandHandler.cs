using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Relations.Commands.RemoveBlocked
{
  public class RemoveBlockedCommandHandler : CommandHandler<RemoveBlockedCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IRelationsRepository _relationsRepository;

    public RemoveBlockedCommandHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(RemoveBlockedCommand request)
    {
      var blockedRelation = await ValidateData(request);

      await _relationsRepository.Update(blockedRelation);

      return Unit.Value;
    }

    private async Task<Relation> ValidateData(RemoveBlockedCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var blockedRelation = await _relationsRepository
        .FindOneByUserIdAndRelatedUserIdAndType(request.UserId, request.BlockedUserId, RelationType.Blocked);

      if (blockedRelation == null)
      {
        throw new NotFoundException(
          $"Entity {nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.BlockedUserId}) not found.");
      }

      return blockedRelation;
    }
  }
}
