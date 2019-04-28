using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class AuthRolesRepository : BaseRepository, IAuthRolesRepository
  {
    public AuthRolesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<UserRole>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.UserRoles
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public void RemoveRangeAsTransaction(IList<UserRole> roles)
    {
      Context.UserRoles.RemoveRange(roles);
    }
  }
}
