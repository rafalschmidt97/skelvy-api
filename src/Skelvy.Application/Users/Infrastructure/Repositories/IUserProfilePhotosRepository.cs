using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUserProfilePhotosRepository : IBaseRepository
  {
    Task<IList<UserProfilePhoto>> FindAllWithAttachmentByProfileId(int profileId);
    Task<IList<UserProfilePhoto>> FindAllWithRemovedByProfilesId(IEnumerable<int> profilesId);
    Task Add(UserProfilePhoto photo);
    Task AddRange(IList<UserProfilePhoto> photos);
    Task RemoveRange(IList<UserProfilePhoto> photos);
  }
}
