using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public class UsersRepository : BaseRepository, IUsersRepository
  {
    public UsersRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<User> FindOneWithDetails(int id)
    {
      return await Context.Users
        .Include(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}
