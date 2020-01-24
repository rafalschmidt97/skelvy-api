using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Infrastructure.Repositories
{
  public interface IUsersRepository : IBaseRepository
  {
    Task<User> FindOne(int id);
    Task<bool> ExistsOne(int id);
    Task<bool> ExistsOneWithRemovedByName(string name);
    Task<User> FindOneWithDetails(int id);
    Task<User> FindOneWithRoles(int userId);
    Task<User> FindOneWithRolesByFacebookId(string facebookId);
    Task<User> FindOneWithRolesByGoogleId(string googleId);
    Task<User> FindOneWithRolesByEmail(string email);
    Task<IList<User>> FindAllRemovedAfterForgottenAtByDate(DateTimeOffset maxDate);
    Task<IList<User>> FindAllWithDetailsByUsersId(IEnumerable<int> usersId);
    Task<IList<User>> FindPageByUserIdAndNameLikeFilterBlocked(int userId, string userName, int pageSize = 10);
    Task<IList<User>> FindAllBetweenId(int minId, int maxId);
    Task Add(User user);
    Task Update(User user);
    Task RemoveRange(IList<User> users);
  }

  public class UserWithRelationType
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public Profile Profile { get; set; }
    public string RelationType { get; set; }
  }
}
