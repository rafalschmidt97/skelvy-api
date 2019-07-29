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

    public async Task<IList<UserProfilePhoto>> FindAllWithAttachmentByProfileId(int profileId)
    {
      return await Context.UserProfilePhotos
        .Include(x => x.Attachment)
        .Where(x => x.ProfileId == profileId)
        .ToListAsync();
    }

    public async Task<IList<UserProfilePhoto>> FindAllWithRemovedByProfilesId(IEnumerable<int> profilesId)
    {
      return await Context.UserProfilePhotos
        .Where(x => profilesId.Any(y => y == x.ProfileId))
        .ToListAsync();
    }

    public async Task Add(UserProfilePhoto photo)
    {
      await Context.UserProfilePhotos.AddAsync(photo);
      await SaveChanges();
    }

    public async Task AddRange(IList<UserProfilePhoto> photos)
    {
      await Context.UserProfilePhotos.AddRangeAsync(photos);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<UserProfilePhoto> photos)
    {
      Context.UserProfilePhotos.RemoveRange(photos);
      await SaveChanges();
    }
  }
}
