using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class ProfilesRepository : BaseRepository, IProfilesRepository
  {
    public ProfilesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<Profile> FindOneByUserId(int userId)
    {
      return await Context.Profiles
        .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<IList<Profile>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.Profiles
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task Add(Profile profile)
    {
      await Context.Profiles.AddAsync(profile);
      await SaveChanges();
    }

    public async Task Update(Profile profile)
    {
      Context.Profiles.Update(profile);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<Profile> profiles)
    {
      Context.Profiles.RemoveRange(profiles);
      await SaveChanges();
    }
  }
}
