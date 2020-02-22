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

namespace Skelvy.Application.Relations.Commands.AddBlocked
{
  public class AddBlockedCommandHandler : CommandHandler<AddBlockedCommand>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly IMediator _mediator;

    public AddBlockedCommandHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository,
      IFriendInvitationsRepository friendInvitationsRepository,
      IMediator mediator)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
      _friendInvitationsRepository = friendInvitationsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(AddBlockedCommand request)
    {
      var relations = await ValidateData(request);
      var friendInvitation = await _friendInvitationsRepository
        .FindOneByInvitingIdAndInvitedIdTwoWay(request.UserId, request.BlockingUserId);

      await using var transaction = _relationsRepository.BeginTransaction();
      foreach (var relation in relations)
      {
        relation.Abort();
      }

      await _relationsRepository.UpdateRange(relations);

      if (friendInvitation != null)
      {
        friendInvitation.Abort();
        await _friendInvitationsRepository.Update(friendInvitation);
      }

      var blockerRelation = new Relation(request.UserId, request.BlockingUserId, RelationType.Blocked);
      await _relationsRepository.Add(blockerRelation);

      transaction.Commit();

      var friendRelationUser = relations.FirstOrDefault(x =>
        x.UserId == request.UserId && x.RelatedUserId == request.BlockingUserId && x.Type == RelationType.Friend);

      if (friendRelationUser != null)
      {
        await _mediator.Publish(new FriendRemovedEvent(request.UserId, request.BlockingUserId));
      }

      return Unit.Value;
    }

    private async Task<IList<Relation>> ValidateData(AddBlockedCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var relatedUserExists = await _usersRepository.ExistsOne(request.BlockingUserId);

      if (!relatedUserExists)
      {
        throw new NotFoundException(nameof(User), request.BlockingUserId);
      }

      var relations = await _relationsRepository
        .FindAllByUserIdAndRelatedUserIdTwoWay(request.UserId, request.BlockingUserId);

      if (relations.Any(x => x.Type == RelationType.Blocked))
      {
        throw new ConflictException(
          $"{nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.BlockingUserId}) already exists.");
      }

      return relations;
    }
  }
}
