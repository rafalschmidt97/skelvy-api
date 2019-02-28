using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveUserCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      _context.Users.Remove(user);

      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}
