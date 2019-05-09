using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

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

    public async Task<User> FindOneWithDetails(int id)
    {
      return await Context.Users
        .Include(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsRemoved);
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

    public async Task<IList<User>> FindAllRemovedAfterForgottenAt(DateTimeOffset maxDate)
    {
      return await Context.Users
        .Where(x => x.IsRemoved && x.ForgottenAt < maxDate)
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
