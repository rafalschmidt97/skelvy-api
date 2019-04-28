using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class UserProfilePhotosRepository : BaseRepository, IUserProfilePhotosRepository
  {
    public UserProfilePhotosRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<UserProfilePhoto>> FindAllByProfileId(int profileId)
    {
      return await Context.UserProfilePhotos
        .Where(x => x.ProfileId == profileId)
        .ToListAsync();
    }

    public async Task<IList<UserProfilePhoto>> FindAllWithRemovedByProfilesId(IEnumerable<int> profilesId)
    {
      return await Context.UserProfilePhotos
        .Where(x => profilesId.Any(y => y == x.ProfileId))
        .ToListAsync();
    }

    public void RemoveRangeAsTransaction(IList<UserProfilePhoto> photos)
    {
      Context.UserProfilePhotos.RemoveRange(photos);
    }

    public void AddRangeAsTransaction(IEnumerable<UserProfilePhoto> photos)
    {
      Context.UserProfilePhotos.AddRange(photos);
    }

    public void AddAsTransaction(UserProfilePhoto photo)
    {
      Context.UserProfilePhotos.Add(photo);
    }
  }
}
