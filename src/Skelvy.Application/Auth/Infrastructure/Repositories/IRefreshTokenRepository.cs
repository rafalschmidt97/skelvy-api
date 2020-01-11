using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Auth.Infrastructure.Repositories
{
  public interface IRefreshTokenRepository : IBaseRepository
  {
    Task<RefreshToken> FindOneByToken(string token);
    Task<IList<RefreshToken>> FindAllByUserId(int userId);
    Task<IList<RefreshToken>> FindAllByUsersId(List<int> usersId);
    Task Add(RefreshToken refreshToken);
    Task Remove(RefreshToken refreshToken);
    Task RemoveRange(IList<RefreshToken> refreshTokens);
  }
}
