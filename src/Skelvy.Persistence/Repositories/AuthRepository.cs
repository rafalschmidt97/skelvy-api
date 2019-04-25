using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class AuthRepository : BaseRepository, IAuthRepository
  {
    public AuthRepository(ISkelvyContext context)
      : base(context)
    {
    }

    public async Task<User> FindOneWithRoles(int userId)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User> FindOneWithRolesByFacebookId(string facebookId)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.FacebookId == facebookId);
    }

    public async Task<User> FindOneWithRolesByGoogleId(string googleId)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.GoogleId == googleId);
    }

    public async Task<User> FindOneWithRolesByEmail(string email)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == email);
    }
  }
}
