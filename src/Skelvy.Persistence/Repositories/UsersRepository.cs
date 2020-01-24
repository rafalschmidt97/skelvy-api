using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Persistence.Repositories
{
  public class UsersRepository : BaseRepository, IUsersRepository
  {
    public UsersRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<User> FindOne(int id)
    {
      return await Context.Users
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsRemoved);
    }

    public async Task<bool> ExistsOne(int id)
    {
      return await Context.Users
        .AnyAsync(x => x.Id == id && !x.IsRemoved);
    }

    public async Task<bool> ExistsOneWithRemovedByName(string name)
    {
      return await Context.Users.AnyAsync(x => x.Name == name);
    }

    public async Task<User> FindOneWithDetails(int id)
    {
      var user = await Context.Users
        .Include(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsRemoved);

      if (user != null)
      {
        var userPhotos = await Context.ProfilePhotos
          .Include(x => x.Attachment)
          .Where(x => x.ProfileId == user.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();

        user.Profile.Photos = userPhotos;
      }

      return user;
    }

    public async Task<User> FindOneWithRoles(int userId)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User> FindOneWithRolesByFacebookId(string facebookId)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.FacebookId == facebookId);
    }

    public async Task<User> FindOneWithRolesByGoogleId(string googleId)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.GoogleId == googleId);
    }

    public async Task<User> FindOneWithRolesByEmail(string email)
    {
      return await Context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<IList<User>> FindAllRemovedAfterForgottenAtByDate(DateTimeOffset maxDate)
    {
      return await Context.Users
        .Where(x => x.IsRemoved && x.ForgottenAt < maxDate)
        .ToListAsync();
    }

    public async Task<IList<User>> FindAllWithDetailsByUsersId(IEnumerable<int> usersId)
    {
      var users = await Context.Users
        .Where(x => usersId.Any(y => y == x.Id))
        .Include(x => x.Profile)
        .ToListAsync();

      foreach (var user in users)
      {
        user.Profile.Photos = await Context.ProfilePhotos
          .Where(x => x.ProfileId == user.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();
      }

      return users;
    }

    public async Task<IList<User>> FindPageByUserIdAndNameLikeFilterBlocked(int userId, string userName, int pageSize = 10)
    {
      var blockedByUsers = await Context.Relations
        .Where(x => x.RelatedUserId == userId && x.Type == RelationType.Blocked && !x.IsRemoved)
        .Select(x => x.UserId)
        .ToListAsync();

      var users = await Context.Users
        .Include(x => x.Profile)
        .Where(x => x.Id != userId &&
                    blockedByUsers.All(y => y != x.Id) &&
                    EF.Functions.Like(x.Name, "%" + userName + "%") &&
                    !x.IsRemoved)
        .OrderBy(x => x.Id)
        .Take(pageSize)
        .ToListAsync();

      foreach (var user in users)
      {
        user.Profile.Photos = await Context.ProfilePhotos
          .Include(x => x.Attachment)
          .Where(x => x.ProfileId == user.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();
      }

      return users;
    }

    public async Task<IList<User>> FindAllBetweenId(int minId, int maxId)
    {
      return await Context.Users
        .Where(x => x.Id >= minId && x.Id <= maxId && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task Add(User user)
    {
      await Context.Users.AddAsync(user);
      await SaveChanges();
    }

    public async Task Update(User user)
    {
      Context.Users.Update(user);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<User> users)
    {
      Context.Users.RemoveRange(users);
      await SaveChanges();
    }
  }
}
