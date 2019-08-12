using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IGroupsRepository : IBaseRepository
  {
    Task<Group> FindOne(int id);
    Task<IList<Group>> FindAllWithUsersDetailsAndMessagesByUserId(int userId, int messagesSageSize = 20);
    Task Add(Group attachment);
    Task Update(Group group);
    Task Remove(Group attachment);
    Task RemoveRange(IList<Group> attachments);
  }
}
