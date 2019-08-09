using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Messages.Infrastructure.Repositories
{
  public interface IMessagesRepository : IBaseRepository
  {
    Task<IList<Message>> FindPageBeforeByGroupId(int groupId, DateTimeOffset beforeDate, int pageSize = 20);
    Task<IList<Message>> FindPageLatestByGroupId(int groupId, int pageSize = 20);
    Task<IList<Message>> FindAllByUsersId(IEnumerable<int> usersId);
    Task<Message> FindOneByActionAndUserIdAndGroupId(string action, int userId, int groupId);
    Task Add(Message message);
    Task Update(Message message);
    Task RemoveRange(IList<Message> messages);
  }
}
