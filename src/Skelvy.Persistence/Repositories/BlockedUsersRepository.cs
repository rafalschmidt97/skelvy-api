using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class BlockedUsersRepository : BaseRepository, IBlockedUsersRepository
  {
    public BlockedUsersRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<BlockedUser>> FindPageWithDetailsByUserId(int userId, int page = 1, int pageSize = 10)
    {
      var skip = (page - 1) * pageSize;
      var users = await Context.BlockedUsers
        .OrderBy(x => x.Id)
        .Skip(skip)
        .Take(pageSize)
        .Include(x => x.BlockUser)
        .ThenInclude(x => x.Profile)
        .Where(x => x.UserId == userId && !x.IsRemoved)
        .ToListAsync();

      foreach (var user in users)
      {
        var userPhotos = await Context.UserProfilePhotos
          .Where(x => x.ProfileId == user.BlockUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();

        user.BlockUser.Profile.Photos = userPhotos;
      }

      return users;
    }

    public async Task<IList<BlockedUser>> FindAllWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.BlockedUsers
        .Include(x => x.BlockUser)
        .ThenInclude(x => x.Profile)
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task<bool> ExistsOneByUserId(int userId, int blockUserId)
    {
      return await Context.BlockedUsers
        .AnyAsync(x => x.UserId == userId && x.BlockUserId == blockUserId && !x.IsRemoved);
    }

    public async Task Add(BlockedUser blockedUser)
    {
      await Context.BlockedUsers.AddAsync(blockedUser);
      await SaveChanges();
    }

    public async Task Update(BlockedUser blockedUser)
    {
      Context.BlockedUsers.Update(blockedUser);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<BlockedUser> blockedUsers)
    {
      Context.BlockedUsers.RemoveRange(blockedUsers);
      await SaveChanges();
    }
  }
}
