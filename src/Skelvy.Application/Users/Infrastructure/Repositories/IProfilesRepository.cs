using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IProfilesRepository : IBaseRepository
  {
    Task<Profile> FindOneByUserId(int userId);
    Task<IList<Profile>> FindAllByUsersId(IEnumerable<int> usersId);
    Task Add(Profile profile);
    Task Update(Profile profile);
    Task RemoveRange(IList<Profile> profiles);
  }
}
