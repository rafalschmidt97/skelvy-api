using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommandHandler : CommandHandler<UpdateUserLanguageCommand>
  {
    private readonly IUsersRepository _usersRepository;

    public UpdateUserLanguageCommandHandler(IUsersRepository usersRepository)
    {
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(UpdateUserLanguageCommand request)
    {
      var user = await _usersRepository.FindOne(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      user.UpdateLanguage(request.Language);

      await _usersRepository.Context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
