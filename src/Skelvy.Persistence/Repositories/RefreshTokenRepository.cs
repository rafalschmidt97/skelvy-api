using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class RefreshTokenRepository : BaseRepository, IRefreshTokenRepository
  {
    public RefreshTokenRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<RefreshToken> FindOneByToken(string token)
    {
      return await Context.RefreshTokens
        .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<IList<RefreshToken>> FindAllByUserId(int userId)
    {
      return await Context.RefreshTokens
        .Where(x => x.UserId == userId)
        .ToListAsync();
    }

    public async Task<IList<RefreshToken>> FindAllByUsersId(List<int> usersId)
    {
      return await Context.RefreshTokens
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task Add(RefreshToken refreshToken)
    {
      await Context.RefreshTokens.AddAsync(refreshToken);
      await SaveChanges();
    }

    public async Task Remove(RefreshToken refreshToken)
    {
      Context.RefreshTokens.Remove(refreshToken);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<RefreshToken> refreshTokens)
    {
      Context.RefreshTokens.RemoveRange(refreshTokens);
      await SaveChanges();
    }
  }
}
