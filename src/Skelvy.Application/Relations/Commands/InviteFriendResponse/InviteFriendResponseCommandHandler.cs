using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Events.UserRespondedFriendInvitation;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Relations.Commands.InviteFriendResponse
{
  public class InviteFriendResponseCommandHandler : CommandHandler<InviteFriendResponseCommand>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMediator _mediator;

    public InviteFriendResponseCommandHandler(
      IRelationsRepository relationsRepository,
      IFriendInvitationsRepository friendInvitationsRepository,
      IUsersRepository usersRepository,
      IMediator mediator)
    {
      _relationsRepository = relationsRepository;
      _friendInvitationsRepository = friendInvitationsRepository;
      _usersRepository = usersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(InviteFriendResponseCommand request)
    {
      var invitation = await ValidateData(request);

      using (var transaction = _relationsRepository.BeginTransaction())
      {
        if (request.IsAccepted)
        {
          invitation.Accept();
        }
        else
        {
          invitation.Deny();
        }

        var relations = new List<Relation>
        {
          new Relation(invitation.InvitingUserId, invitation.InvitedUserId, RelationType.Friend),
          new Relation(invitation.InvitedUserId, invitation.InvitingUserId, RelationType.Friend),
        };

        await _friendInvitationsRepository.Update(invitation);
        await _relationsRepository.AddRange(relations);

        transaction.Commit();

        await _mediator.Publish(
          new UserRespondedFriendInvitationEvent(invitation.Id, request.IsAccepted, invitation.InvitingUserId, invitation.InvitedUserId));
      }

      return Unit.Value;
    }

    private async Task<FriendInvitation> ValidateData(InviteFriendResponseCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException($"{nameof(Profile)}(UserId = {request.UserId}) not found.");
      }

      var invitation = await _friendInvitationsRepository.FindOneByRequestId(request.RequestId);

      if (invitation == null)
      {
        throw new NotFoundException(nameof(FriendInvitation), request.RequestId);
      }

      if (invitation.InvitedUserId != request.UserId)
      {
        throw new ConflictException(
          $"{nameof(FriendInvitation)}({request.RequestId}) does not belong to {nameof(User)}({request.UserId}).");
      }

      var existsBlockedRelation = await _relationsRepository
        .ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(invitation.InvitedUserId, invitation.InvitingUserId, RelationType.Blocked);

      if (existsBlockedRelation)
      {
        throw new ConflictException(
          $"{nameof(User)}({invitation.InvitedUserId}) is blocked/blocking {nameof(User)}({invitation.InvitingUserId}).");
      }

      return invitation;
    }
  }
}
