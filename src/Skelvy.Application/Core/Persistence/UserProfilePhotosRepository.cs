using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public class UserProfilePhotosRepository : BaseRepository, IUserProfilePhotosRepository
  {
    public UserProfilePhotosRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<UserProfilePhoto>> FindAllByProfilesId(IEnumerable<int> profilesId)
    {
      return await Context.UserProfilePhotos
        .Where(x => profilesId.Any(y => y == x.ProfileId))
        .ToListAsync();
    }
  }
}
