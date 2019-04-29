using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUserProfilePhotosRepository : IBaseRepository
  {
    Task<IList<UserProfilePhoto>> FindAllByProfileId(int profileId);
    Task<IList<UserProfilePhoto>> FindAllWithRemovedByProfilesId(IEnumerable<int> profilesId);
    Task Add(UserProfilePhoto photo);
    Task AddRange(IEnumerable<UserProfilePhoto> newPhotos);
    Task RemoveRange(IList<UserProfilePhoto> oldPhotos);
  }
}
