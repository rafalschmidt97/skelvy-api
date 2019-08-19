using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Infrastructure.Repositories
{
  public interface IRelationsRepository : IBaseRepository
  {
    Task<Relation> FindOneByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type);
    Task<Relation> FindOneByUserIdAndRelatedUserIdTwoWay(int userId, int relatedUserId);
    Task<IList<Relation>> FindAllByUserIdAndRelatedUserIdAndTypeTwoWay(int userId, int relatedUserId, string type);
    Task<IList<Relation>> FindPageUsersWithRelatedDetailsByUserIdAndType(int userId, string relationType, int page, int pageSize = 10);
    Task<bool> ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(int userId, int relatedUserId, string type);
    Task<bool> ExistsOneByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type);
    Task<IList<Relation>> FindAllWithRemovedByUsersId(List<int> usersId);
    Task Add(Relation relation);
    Task AddRange(IList<Relation> relations);
    Task Update(Relation relation);
    Task UpdateRange(IList<Relation> relations);
    Task RemoveRange(IList<Relation> relations);
  }
}
