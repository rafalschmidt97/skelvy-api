using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Infrastructure.Repositories
{
  public interface IFriendRequestsRepository : IBaseRepository
  {
    Task<IList<FriendRequest>> FindAllWithInvitingDetailsByUserId(int userId);
    Task<FriendRequest> FindOneByRequestId(int requestId);
    Task<bool> ExistsOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId);
    Task<IList<FriendRequest>> FindAllWithRemovedByUsersId(List<int> usersId);
    Task Add(FriendRequest friendsRequest);
    Task Update(FriendRequest request);
    Task RemoveRange(IList<FriendRequest> friendRequests);
  }
}
