using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Events.UserSentFriendRequest;
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
    private readonly IFriendRequestsRepository _friendRequestsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMediator _mediator;

    public InviteFriendCommandHandler(
      IRelationsRepository relationsRepository,
      IFriendRequestsRepository friendRequestsRepository,
      IUsersRepository usersRepository,
      IMediator mediator)
    {
      _relationsRepository = relationsRepository;
      _friendRequestsRepository = friendRequestsRepository;
      _usersRepository = usersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(InviteFriendCommand request)
    {
      await ValidateData(request);

      var friendRequest = new FriendRequest(request.UserId, request.InvitingUserId);

      await _friendRequestsRepository.Add(friendRequest);

      await _mediator.Publish(
        new UserSentFriendRequestEvent(friendRequest.Id, friendRequest.InvitingUserId, friendRequest.InvitedUserId));

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

      var requestExists = await _friendRequestsRepository
        .ExistsOneByInvitingIdAndInvitedIdTwoWay(request.UserId, request.InvitingUserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"{nameof(FriendRequest)}(InvitingUserId={request.UserId}, InvitedUserId={request.InvitingUserId}) already exists.");
      }
    }
  }
}
