using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.AddBlockedUser
{
  public class AddBlockedUserCommandHandler : CommandHandler<AddBlockedUserCommand>
  {
    private readonly IBlockedUsersRepository _blockedUsersRepository;
    private readonly IUsersRepository _usersRepository;

    public AddBlockedUserCommandHandler(IBlockedUsersRepository blockedUsersRepository, IUsersRepository usersRepository)
    {
      _blockedUsersRepository = blockedUsersRepository;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(AddBlockedUserCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var blockedUserExists = await _blockedUsersRepository.ExistsOneByUserId(request.UserId, request.BlockUserId);

      if (blockedUserExists)
      {
        throw new ConflictException(
          $"Entity {nameof(BlockedUser)}(UserId = {request.UserId}, BlockUserId = {request.BlockUserId}) already exists.");
      }

      var blockUser = new BlockedUser(request.UserId, request.BlockUserId);
      await _blockedUsersRepository.Add(blockUser);

      return Unit.Value;
    }
  }
}
