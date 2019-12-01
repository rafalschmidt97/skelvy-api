using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class GroupsRepository : BaseRepository, IGroupsRepository
  {
    public GroupsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<Group> FindOne(int id)
    {
      return await Context.Groups.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Group>> FindAllWithUsersDetailsAndMessagesByUserId(int userId, int messagesPageSize = 20)
    {
      var groups = await Context.Groups
        .Include(y => y.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Where(x => x.Users.Any(y => y.UserId == userId && !y.IsRemoved) && !x.IsRemoved)
        .ToListAsync();

      groups.ForEach(x => x.Users = x.Users.Where(y => !y.IsRemoved).ToList());

      foreach (var group in groups)
      {
        foreach (var groupUser in group.Users)
        {
          var userPhotos = await Context.ProfilePhotos
            .Include(x => x.Attachment)
            .Where(x => x.ProfileId == groupUser.User.Profile.Id)
            .OrderBy(x => x.Order)
            .ToListAsync();

          groupUser.User.Profile.Photos = userPhotos;
        }

        var messages = await Context.Messages
          .Where(x => x.GroupId == group.Id)
          .OrderByDescending(p => p.Date)
          .Take(messagesPageSize)
          .ToListAsync();

        group.Messages = messages.OrderBy(x => x.Date).ToList();
      }

      return groups;
    }

    public async Task<Group> FindOneWithUsersDetailsAndMessagesByGroupIdAndUserId(int groupId, int userId, int messagesPageSize = 20)
    {
      var group = await Context.Groups
        .Include(y => y.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == groupId &&
                                  x.Users.Any(y => y.UserId == userId && !y.IsRemoved) && !x.IsRemoved);

      if (group != null)
      {
        group.Users = group.Users.Where(y => !y.IsRemoved).ToList();

        foreach (var groupUser in group.Users)
        {
          var userPhotos = await Context.ProfilePhotos
            .Include(x => x.Attachment)
            .Where(x => x.ProfileId == groupUser.User.Profile.Id)
            .OrderBy(x => x.Order)
            .ToListAsync();

          groupUser.User.Profile.Photos = userPhotos;
        }

        var messages = await Context.Messages
          .Where(x => x.GroupId == group.Id)
          .OrderByDescending(p => p.Date)
          .Take(messagesPageSize)
          .ToListAsync();

        group.Messages = messages.OrderBy(x => x.Date).ToList();
      }

      return group;
    }

    public async Task Add(Group attachment)
    {
      await Context.Groups.AddAsync(attachment);
      await SaveChanges();
    }

    public async Task Update(Group group)
    {
      Context.Groups.Update(group);
      await SaveChanges();
    }

    public async Task Remove(Group attachment)
    {
      Context.Groups.Remove(attachment);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<Group> attachments)
    {
      Context.Groups.RemoveRange(attachments);
      await SaveChanges();
    }
  }
}
