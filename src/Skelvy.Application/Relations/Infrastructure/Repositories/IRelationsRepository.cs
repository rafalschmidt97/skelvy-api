using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Infrastructure.Repositories
{
  public interface IRelationsRepository : IBaseRepository
  {
    Task<IList<Relation>> FindAllRelationsByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type);
    Task<IList<Relation>> FindPageRelationsUsersWithRelatedDetailsByUserIdAndType(int userId, string relationType, int page, int pageSize = 10);
    Task<IList<FriendRequest>> FindAllFriendRequestsWithInvitingDetailsByUserId(int userId);
    Task<FriendRequest> FindOneFriendRequestByRequestId(int requestId);
    Task<bool> ExistsFriendRequestByInvitingIdAndInvitedId(int invitingUserId, int invitedUserId);
    Task<bool> ExistsRelationByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type);
    Task<IList<Relation>> FindAllRelationsWithRemovedByUsersId(List<int> usersId);
    Task<IList<FriendRequest>> FindAllFriendRequestWithRemovedByUsersId(List<int> usersId);
    Task AddRangeRelations(IList<Relation> relations);
    Task AddFriendRequest(FriendRequest friendsRequest);
    Task UpdateFriendsRequest(FriendRequest request);
    Task UpdateRelation(Relation relation);
    Task UpdateRangeRelation(IList<Relation> relations);
    Task RemoveRangeRelation(IList<Relation> relations);
    Task RemoveRangeFriendRequest(IList<FriendRequest> friendRequests);
  }
}
