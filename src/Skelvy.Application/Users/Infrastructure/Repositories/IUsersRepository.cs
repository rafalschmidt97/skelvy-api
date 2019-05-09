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
    Task<User> FindOneWithDetails(int id);
    Task<User> FindOneWithRoles(int userId);
    Task<User> FindOneWithRolesByFacebookId(string facebookId);
    Task<User> FindOneWithRolesByGoogleId(string googleId);
    Task<User> FindOneWithRolesByEmail(string email);
    Task<IList<User>> FindAllRemovedAfterForgottenAt(DateTimeOffset maxDate);
    Task Add(User user);
    Task Update(User user);
    Task RemoveRange(IList<User> users);
  }
}
