using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Groups.Infrastructure.Repositories
{
  public interface IGroupUsersRepository : IBaseRepository
  {
    Task<bool> ExistsOneByUserIdAndGroupId(int userId, int groupId);
    Task<bool> ExistsOneByUserIdAndGroupIdAndRole(int userId, int groupId, string role);
    Task<GroupUser> FindOneByUserIdAndGroupId(int userId, int groupId);
    Task<bool> ExistsOneByMeetingRequestId(int requestId);
    Task<GroupUser> FindOneWithGroupByUserIdAndGroupId(int userId, int groupId);
    Task<IList<GroupUser>> FindAllByGroupId(int groupId);
    Task<IList<GroupUser>> FindAllByUserId(int userId);
    Task<IList<GroupUser>> FindAllWithGroupAndRequestByGroupId(int groupId);
    Task<IList<GroupUser>> FindAllWithRequestByGroupsId(IEnumerable<int> groupId);
    Task<IList<GroupUser>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId);
    Task<IList<GroupUser>> FindAllWithRemovedAfterOrEqualAbortedAtByGroupId(int groupId, DateTimeOffset leftAt);
    Task<IList<GroupUser>> FindAllWithExpiredByGroupId(int groupId);
    Task Add(GroupUser groupUser);
    Task AddRange(IList<GroupUser> groupUsers);
    Task Update(GroupUser groupUser);
    Task UpdateRange(IList<GroupUser> groupUsers);
    Task RemoveRange(IList<GroupUser> groupUsers);
  }
}
