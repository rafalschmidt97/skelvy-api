using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUserRolesRepository : IBaseRepository
  {
    Task<IList<UserRole>> FindAllByUsersId(IEnumerable<int> usersId);
    Task RemoveRange(IList<UserRole> roles);
  }
}
