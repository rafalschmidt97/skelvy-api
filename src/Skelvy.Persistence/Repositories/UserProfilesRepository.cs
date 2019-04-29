using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class UserProfilesRepository : BaseRepository, IUserProfilesRepository
  {
    public UserProfilesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<UserProfile> FindOneByUserId(int userId)
    {
      return await Context.UserProfiles
        .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<IList<UserProfile>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.UserProfiles
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task Add(UserProfile profile)
    {
      await Context.UserProfiles.AddAsync(profile);
      await SaveChanges();
    }

    public async Task Update(UserProfile profile)
    {
      Context.UserProfiles.Update(profile);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<UserProfile> profiles)
    {
      Context.UserProfiles.RemoveRange(profiles);
      await SaveChanges();
    }
  }
}
