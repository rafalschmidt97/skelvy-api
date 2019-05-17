using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IBlockedUsersRepository
  {
    Task<IList<BlockedUser>> FindPageWithDetailsByUserId(int userId, int page = 1, int pageSize = 10);
    Task<IList<BlockedUser>> FindAllWithRemovedByUsersId(List<int> usersId);
    Task<bool> ExistsOneByUserId(int userId, int blockUserId);
    Task Add(BlockedUser user);
    Task Update(BlockedUser user);
    Task RemoveRange(IList<BlockedUser> users);
  }
}
