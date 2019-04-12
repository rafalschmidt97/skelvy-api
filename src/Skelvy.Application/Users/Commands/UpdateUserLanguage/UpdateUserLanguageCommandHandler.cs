using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommandHandler : CommandHandler<UpdateUserLanguageCommand>
  {
    private readonly SkelvyContext _context;

    public UpdateUserLanguageCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public override async Task<Unit> Handle(UpdateUserLanguageCommand request)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      user.Language = request.Language;

      await _context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
