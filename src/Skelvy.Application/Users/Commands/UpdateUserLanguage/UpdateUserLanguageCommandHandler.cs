using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommandHandler : IRequestHandler<UpdateUserLanguageCommand>
  {
    private readonly SkelvyContext _context;

    public UpdateUserLanguageCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(UpdateUserLanguageCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      user.Language = request.Language;

      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}
