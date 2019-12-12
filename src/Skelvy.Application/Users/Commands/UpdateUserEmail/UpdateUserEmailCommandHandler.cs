using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.UpdateUserEmail
{
  public class UpdateUserEmailCommandHandler : CommandHandler<UpdateUserEmailCommand>
  {
    private readonly IUsersRepository _usersRepository;

    public UpdateUserEmailCommandHandler(IUsersRepository usersRepository)
    {
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(UpdateUserEmailCommand request)
    {
      var user = await _usersRepository.FindOne(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      user.UpdateEmail(request.Email);

      await _usersRepository.Update(user);
      return Unit.Value;
    }
  }
}
