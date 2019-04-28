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
    void RemoveRangeAsTransaction(IList<UserProfilePhoto> oldPhotos);
    void AddRangeAsTransaction(IEnumerable<UserProfilePhoto> newPhotos);
    void AddAsTransaction(UserProfilePhoto photo);
  }
}
