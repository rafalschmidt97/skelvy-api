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
      await ValidateData(request);

      var relation = new Relation(request.UserId, request.RelatedUserId, RelationType.Blocked);
      await _relationsRepository.Add(relation);

      return Unit.Value;
    }

    private async Task ValidateData(AddBlockedCommand request)
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

      var relationExists = await _relationsRepository
        .ExistsByUserIdAndRelatedUserIdAndType(request.UserId, request.RelatedUserId, RelationType.Blocked);

      if (relationExists)
      {
        throw new ConflictException(
          $"Entity {nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.RelatedUserId}) already exists.");
      }
    }
  }
}
