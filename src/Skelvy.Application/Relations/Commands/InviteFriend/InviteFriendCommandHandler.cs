using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Events.UserSentFriendInvitation;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Relations.Commands.InviteFriend
{
  public class InviteFriendCommandHandler : CommandHandler<InviteFriendCommand>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMediator _mediator;

    public InviteFriendCommandHandler(
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

    public override async Task<Unit> Handle(InviteFriendCommand request)
    {
      await ValidateData(request);

      var invitation = new FriendInvitation(request.UserId, request.InvitingUserId);

      await _friendInvitationsRepository.Add(invitation);

      await _mediator.Publish(
        new UserSentFriendInvitationEvent(invitation.Id, invitation.InvitingUserId, invitation.InvitedUserId));

      return Unit.Value;
    }

    private async Task ValidateData(InviteFriendCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var relatedUserExists = await _usersRepository.ExistsOne(request.InvitingUserId);

      if (!relatedUserExists)
      {
        throw new NotFoundException(nameof(User), request.InvitingUserId);
      }

      var existsFriendRelation = await _relationsRepository
        .ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(request.UserId, request.InvitingUserId, RelationType.Friend);

      if (existsFriendRelation)
      {
        throw new ConflictException(
          $"{nameof(Relation)}(UserId={request.UserId}, RelatedUserId={request.InvitingUserId}) already exists.");
      }

      var existsBlockedRelation = await _relationsRepository
        .ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(request.UserId, request.InvitingUserId, RelationType.Blocked);

      if (existsBlockedRelation)
      {
        throw new ConflictException(
          $"{nameof(User)}({request.UserId}) is blocked/blocking {nameof(User)}({request.InvitingUserId}).");
      }

      var requestExists = await _friendInvitationsRepository
        .ExistsOneByInvitingIdAndInvitedIdTwoWay(request.UserId, request.InvitingUserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"{nameof(FriendInvitation)}(InvitingUserId={request.UserId}, InvitedUserId={request.InvitingUserId}) already exists.");
      }
    }
  }
}
