using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUsersRepository : IBaseRepository
  {
    Task<User> FindOneWithDetails(int id);
  }
}
