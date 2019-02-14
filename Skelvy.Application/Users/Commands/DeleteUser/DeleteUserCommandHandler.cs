using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.DeleteUser
{
  public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
  {
    private readonly SkelvyContext _context;

    public DeleteUserCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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
