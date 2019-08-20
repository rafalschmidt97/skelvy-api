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

namespace Skelvy.Application.Relations.Commands.AddBlocked
{
  public class AddBlockedCommandHandler : CommandHandler<AddBlockedCommand>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IUsersRepository _usersRepository;

    public AddBlockedCommandHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(AddBlockedCommand request)
    {
      var relations = await ValidateData(request);

      using (var transaction = _relationsRepository.BeginTransaction())
      {
        foreach (var relation in relations)
        {
          relation.Abort();
        }

        await _relationsRepository.UpdateRange(relations);

        var blockerRelation = new Relation(request.UserId, request.RelatedUserId, RelationType.Blocked);
        await _relationsRepository.Add(blockerRelation);

        transaction.Commit();
      }

      return Unit.Value;
    }

    private async Task<IList<Relation>> ValidateData(AddBlockedCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException($"Entity {nameof(User)}(UserId = {request.UserId}) not found.");
      }

      var relatedUserExists = await _usersRepository.ExistsOne(request.RelatedUserId);

      if (!relatedUserExists)
      {
        throw new NotFoundException($"Entity {nameof(User)}(UserId = {request.RelatedUserId}) not found.");
      }

      var relations = await _relationsRepository
        .FindAllByUserIdAndRelatedUserIdTwoWay(request.UserId, request.RelatedUserId);

      if (relations.Any(x => x.Type == RelationType.Blocked))
      {
        throw new ConflictException(
          $"Entity {nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.RelatedUserId}) already exists.");
      }

      return relations;
    }
  }
}
