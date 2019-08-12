using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IGroupUsersRepository : IBaseRepository
  {
    Task<bool> ExistsOneByUserIdAndGroupId(int userId, int groupId);
    Task<bool> ExistsOneByMeetingRequest(int requestId);
    Task<GroupUser> FindOneWithGroupByUserIdAndGroupId(int userId, int groupId);
    Task<IList<GroupUser>> FindAllByGroupId(int groupId);
    Task<IList<GroupUser>> FindAllByUserId(int userId);
    Task<IList<GroupUser>> FindAllWithRequestByGroupId(int groupId);
    Task<IList<GroupUser>> FindAllWithRequestByGroupsId(IEnumerable<int> groupId);
    Task<IList<GroupUser>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId);
    Task<IList<GroupUser>> FindAllWithRemovedAfterOrEqualAbortedAtByGroupId(int groupId, DateTimeOffset leftAt);
    Task<IList<GroupUser>> FindAllWithExpiredByGroupId(int groupId);
    Task<IList<GroupUser>> FindAllWithGroupByGroupId(int groupId);
    Task<bool> ExistsOneByUserId(int userId);
    Task Add(GroupUser groupUser);
    Task AddRange(IList<GroupUser> groupUsers);
    Task Update(GroupUser groupUser);
    Task RemoveRange(IList<GroupUser> groupUsers);
  }
}
