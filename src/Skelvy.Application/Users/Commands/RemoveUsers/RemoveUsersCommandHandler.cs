using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUsers
{
  public class RemoveUsersCommandHandler : CommandHandler<RemoveUsersCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveUsersCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public override async Task<Unit> Handle(RemoveUsersCommand request)
    {
      var today = DateTimeOffset.UtcNow;

      var usersToRemove = await _context.Users
        .Where(x => x.IsDeleted && x.DeletionDate < today)
        .ToListAsync();

      _context.Users.RemoveRange(usersToRemove);
      await _context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
