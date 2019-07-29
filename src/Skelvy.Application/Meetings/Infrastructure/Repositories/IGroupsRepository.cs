using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IGroupsRepository : IBaseRepository
  {
    Task<Group> FindOneByGroupId(int id);
    Task Add(Group attachment);
    Task Update(Group group);
    Task Remove(Group attachment);
    Task RemoveRange(IList<Group> attachments);
  }
}
