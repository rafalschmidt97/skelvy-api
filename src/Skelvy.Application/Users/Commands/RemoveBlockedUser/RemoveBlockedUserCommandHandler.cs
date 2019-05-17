using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.RemoveBlockedUser
{
  public class RemoveBlockedUserCommandHandler : CommandHandler<RemoveBlockedUserCommand>
  {
    private readonly IBlockedUsersRepository _blockedUsersRepository;
    private readonly IUsersRepository _usersRepository;

    public RemoveBlockedUserCommandHandler(IBlockedUsersRepository blockedUsersRepository, IUsersRepository usersRepository)
    {
      _blockedUsersRepository = blockedUsersRepository;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(RemoveBlockedUserCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var blockedUser = await _blockedUsersRepository.FindOneByUserId(request.UserId, request.BlockUserId);

      if (blockedUser == null)
      {
        throw new NotFoundException($"Entity {nameof(BlockedUser)}(UserId = {request.UserId}, BlockUserId = {request.BlockUserId}) not found.");
      }

      blockedUser.Remove();
      await _blockedUsersRepository.Update(blockedUser);

      return Unit.Value;
    }
  }
}
