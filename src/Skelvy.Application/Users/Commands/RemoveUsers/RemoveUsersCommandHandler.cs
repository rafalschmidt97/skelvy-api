using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUsers
{
  public class RemoveUsersCommandHandler : IRequestHandler<RemoveUsersCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveUsersCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(RemoveUsersCommand request, CancellationToken cancellationToken)
    {
      var today = DateTimeOffset.UtcNow;

      var usersToRemove = await _context.Users
        .Where(x => x.IsDeleted && x.DeletionDate < today)
        .ToListAsync(cancellationToken);

      _context.Users.RemoveRange(usersToRemove);
      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
