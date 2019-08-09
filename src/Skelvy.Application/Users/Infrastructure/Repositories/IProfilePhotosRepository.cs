using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IProfilePhotosRepository : IBaseRepository
  {
    Task<IList<ProfilePhoto>> FindAllWithAttachmentByProfileId(int profileId);
    Task<IList<ProfilePhoto>> FindAllWithRemovedByProfilesId(IEnumerable<int> profilesId);
    Task Add(ProfilePhoto photo);
    Task AddRange(IList<ProfilePhoto> photos);
    Task RemoveRange(IList<ProfilePhoto> photos);
  }
}
