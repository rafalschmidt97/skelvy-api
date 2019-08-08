using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Events.UserRespondedFriendRequest;
using Skelvy.Application.Relations.Infrastructure;
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
    private readonly IUsersRepository _usersRepository;
    private readonly IMediator _mediator;

    public InviteFriendResponseCommandHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository,
      IMediator mediator)
    {
      _relationsRepository = relationsRepository;
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

        await _relationsRepository.UpdateFriendsRequest(friendRequest);
        await _relationsRepository.AddRangeRelations(relations);

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
        throw new NotFoundException($"Entity {nameof(UserProfile)}(UserId = {request.UserId}) not found.");
      }

      var friendRequest = await _relationsRepository.FindOneFriendRequestByRequestId(request.RequestId);

      if (friendRequest == null)
      {
        throw new NotFoundException(nameof(FriendRequest), request.RequestId);
      }

      if (friendRequest.InvitedUserId != request.UserId)
      {
        throw new ConflictException(
          $"Request {nameof(FriendRequest)}(RequestId = {request.RequestId}) not belong to {nameof(User)}(Id = {request.UserId}).");
      }

      return friendRequest;
    }
  }
}
