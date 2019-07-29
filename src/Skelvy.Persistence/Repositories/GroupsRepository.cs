using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class GroupsRepository : BaseRepository, IGroupsRepository
  {
    public GroupsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<Group> FindOneByGroupId(int id)
    {
      return await Context.Groups.FirstOrDefaultAsync(x => x.Id == id);
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
