using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class ProfilePhotosRepository : BaseRepository, IProfilePhotosRepository
  {
    public ProfilePhotosRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<ProfilePhoto>> FindAllWithAttachmentByProfileId(int profileId)
    {
      return await Context.ProfilePhotos
        .Include(x => x.Attachment)
        .Where(x => x.ProfileId == profileId)
        .ToListAsync();
    }

    public async Task<IList<ProfilePhoto>> FindAllWithRemovedByProfilesId(IEnumerable<int> profilesId)
    {
      return await Context.ProfilePhotos
        .Where(x => profilesId.Any(y => y == x.ProfileId))
        .ToListAsync();
    }

    public async Task Add(ProfilePhoto photo)
    {
      await Context.ProfilePhotos.AddAsync(photo);
      await SaveChanges();
    }

    public async Task AddRange(IList<ProfilePhoto> photos)
    {
      await Context.ProfilePhotos.AddRangeAsync(photos);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<ProfilePhoto> photos)
    {
      Context.ProfilePhotos.RemoveRange(photos);
      await SaveChanges();
    }
  }
}
