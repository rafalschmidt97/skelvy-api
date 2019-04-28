using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Auth.Infrastructure.Repositories
{
  public interface IAuthRolesRepository : IBaseRepository
  {
    Task<IList<UserRole>> FindAllByUsersId(IEnumerable<int> usersId);
    void RemoveRangeAsTransaction(IList<UserRole> roles);
  }
}
