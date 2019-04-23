using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public class UserProfilesRepository : BaseRepository, IUserProfilesRepository
  {
    public UserProfilesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<UserProfile>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.UserProfiles
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }
  }
}
