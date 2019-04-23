using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUserProfilesRepository : IBaseRepository
  {
    Task<IList<UserProfile>> FindAllByUsersId(IEnumerable<int> usersId);
  }
}
