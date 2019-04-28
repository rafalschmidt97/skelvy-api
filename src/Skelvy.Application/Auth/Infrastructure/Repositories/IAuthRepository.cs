using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Auth.Infrastructure.Repositories
{
  public interface IAuthRepository : IBaseRepository
  {
    Task<User> FindOneWithRoles(int userId);
    Task<User> FindOneWithRolesByFacebookId(string facebookId);
    Task<User> FindOneWithRolesByGoogleId(string googleId);
    Task<User> FindOneWithRolesByEmail(string email);
    void AddAsTransaction(User user);
    Task Update(User user);
  }
}
