using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class UserRolesRepository : BaseRepository, IUserRolesRepository
  {
    public UserRolesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<UserRole>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.UserRoles
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task RemoveRange(IList<UserRole> roles)
    {
      Context.UserRoles.RemoveRange(roles);
      await SaveChanges();
    }
  }
}
