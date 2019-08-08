using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Infrastructure.Repositories
{
  public interface IRelationsRepository : IBaseRepository
  {
    Task<IList<Relation>> FindAllByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type);
    Task<IList<Relation>> FindPageUsersWithRelatedDetailsByUserIdAndType(int userId, string relationType, int page, int pageSize = 10);
    Task<bool> ExistsByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type);
    Task<IList<Relation>> FindAllWithRemovedByUsersId(List<int> usersId);
    Task AddRange(IList<Relation> relations);
    Task Update(Relation relation);
    Task UpdateRange(IList<Relation> relations);
    Task RemoveRange(IList<Relation> relations);
  }
}
