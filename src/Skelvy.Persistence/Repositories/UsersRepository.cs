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

    public async Task<IList<User>> FindAllRemovedAfterForgottenAt(DateTimeOffset maxDate)
    {
      return await Context.Users
        .Where(x => x.IsRemoved && x.ForgottenAt < maxDate)
        .ToListAsync();
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