using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class GroupUsersRepository : BaseRepository, IGroupUsersRepository
  {
    public GroupUsersRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<GroupUser> FindOneByUserId(int userId)
    {
      return await Context.GroupUsers
        .Include(x => x.Group)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved && !x.Group.IsRemoved);
    }

    public async Task<GroupUser> FindOneWithGroupByUserId(int userId)
    {
      return await Context.GroupUsers
        .Include(x => x.Group)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved && !x.Group.IsRemoved);
    }

    public async Task<IList<GroupUser>> FindAllByGroupId(int groupId)
    {
      return await Context.GroupUsers
        .Include(x => x.Group)
        .Where(x => x.GroupId == groupId && !x.IsRemoved && !x.Group.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<GroupUser>> FindAllWithRequestByGroupId(int groupId)
    {
      return await Context.GroupUsers
        .Include(x => x.Group)
        .Include(x => x.MeetingRequest)
        .Where(x => x.GroupId == groupId && !x.IsRemoved && !x.Group.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<GroupUser>> FindAllWithRequestByGroupsId(IEnumerable<int> groupId)
    {
      return await Context.GroupUsers
        .Include(x => x.Group)
        .Include(x => x.MeetingRequest)
        .Where(x => groupId.Any(y => y == x.GroupId) && !x.IsRemoved && !x.Group.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<GroupUser>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId)
    {
      return await Context.GroupUsers
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task<IList<GroupUser>> FindAllWithRemovedAfterOrEqualAbortedAtByGroupId(int groupId, DateTimeOffset leftAt)
    {
      return await Context.GroupUsers
        .Where(x => x.GroupId == groupId && x.ModifiedAt >= leftAt)
        .ToListAsync();
    }

    public async Task<IList<GroupUser>> FindAllWithExpiredByGroupId(int groupId)
    {
      return await Context.GroupUsers
        .Where(x => x.GroupId == groupId && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<bool> ExistsOneByUserId(int userId)
    {
      var meetingUser = await Context.GroupUsers
        .Include(x => x.Group)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved && !x.Group.IsRemoved);

      return meetingUser != null;
    }

    public async Task Add(GroupUser groupUser)
    {
      await Context.GroupUsers.AddAsync(groupUser);
      await SaveChanges();
    }

    public async Task AddRange(IList<GroupUser> groupUsers)
    {
      await Context.GroupUsers.AddRangeAsync(groupUsers);
      await SaveChanges();
    }

    public async Task Update(GroupUser groupUser)
    {
      Context.GroupUsers.Update(groupUser);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<GroupUser> groupUsers)
    {
      Context.GroupUsers.RemoveRange(groupUsers);
      await SaveChanges();
    }
  }
}
