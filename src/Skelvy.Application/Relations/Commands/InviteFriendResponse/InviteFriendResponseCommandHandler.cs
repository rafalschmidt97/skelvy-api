using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Events.UserRespondedFriendRequest;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Relations.Commands.InviteFriendResponse
{
  public class InviteFriendResponseCommandHandler : CommandHandler<InviteFriendResponseCommand>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IFriendRequestsRepository _friendRequestsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMediator _mediator;

    public InviteFriendResponseCommandHandler(
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

    public override async Task<Unit> Handle(InviteFriendResponseCommand request)
    {
      var friendRequest = await ValidateData(request);

      using (var transaction = _relationsRepository.BeginTransaction())
      {
        if (request.IsAccepted)
        {
          friendRequest.Accept();
        }
        else
        {
          friendRequest.Deny();
        }

        var relations = new List<Relation>
        {
          new Relation(friendRequest.InvitingUserId, friendRequest.InvitedUserId, RelationType.Friend),
          new Relation(friendRequest.InvitedUserId, friendRequest.InvitingUserId, RelationType.Friend),
        };

        await _friendRequestsRepository.Update(friendRequest);
        await _relationsRepository.AddRange(relations);

        transaction.Commit();

        await _mediator.Publish(
          new UserRespondedFriendRequestEvent(friendRequest.Id, request.IsAccepted, friendRequest.InvitingUserId, friendRequest.InvitedUserId));
      }

      return Unit.Value;
    }

    private async Task<FriendRequest> ValidateData(InviteFriendResponseCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException($"Entity {nameof(Profile)}(UserId = {request.UserId}) not found.");
      }

      var friendRequest = await _friendRequestsRepository.FindOneByRequestId(request.RequestId);

      if (friendRequest == null)
      {
        throw new NotFoundException(nameof(FriendRequest), request.RequestId);
      }

      if (friendRequest.InvitedUserId != request.UserId)
      {
        throw new ConflictException(
          $"Request {nameof(FriendRequest)}(RequestId = {request.RequestId}) not belong to {nameof(User)}(Id = {request.UserId}).");
      }

      var existsBlockedRelation = await _relationsRepository
        .ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(friendRequest.InvitedUserId, friendRequest.InvitingUserId, RelationType.Blocked);

      if (existsBlockedRelation)
      {
        throw new ConflictException(
          $"Entity {nameof(User)}(UserId={friendRequest.InvitedUserId}) is blocked/blocking {nameof(User)}(UserId={friendRequest.InvitingUserId}).");
      }

      return friendRequest;
    }
  }
}
