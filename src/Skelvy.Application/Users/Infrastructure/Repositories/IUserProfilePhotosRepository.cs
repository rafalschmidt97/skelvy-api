using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUserProfilePhotosRepository : IBaseRepository
  {
    Task<IList<UserProfilePhoto>> FindAllByProfilesId(IEnumerable<int> profilesId);
  }
}
