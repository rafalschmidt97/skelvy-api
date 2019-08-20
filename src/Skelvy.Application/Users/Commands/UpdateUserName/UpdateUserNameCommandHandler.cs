using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.UpdateUserName
{
  public class UpdateUserNameCommandHandler : CommandHandler<UpdateUserNameCommand>
  {
    private readonly IUsersRepository _usersRepository;

    public UpdateUserNameCommandHandler(IUsersRepository usersRepository)
    {
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(UpdateUserNameCommand request)
    {
      var user = await _usersRepository.FindOne(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var existsUserName = await _usersRepository.ExistsOneWithRemovedByName(request.Name);

      if (existsUserName)
      {
        throw new ConflictException($"{nameof(User)}(Name = {request.Name}) already exists.");
      }

      user.UpdateName(request.Name);

      await _usersRepository.Update(user);
      return Unit.Value;
    }
  }
}
