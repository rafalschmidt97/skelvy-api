using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUserProfilesRepository : IBaseRepository
  {
    Task<UserProfile> FindOneByUserId(int userId);
    Task<IList<UserProfile>> FindAllByUsersId(IEnumerable<int> usersId);
    Task Add(UserProfile profile);
    Task Update(UserProfile profile);
    Task RemoveRange(IList<UserProfile> profiles);
  }
}
