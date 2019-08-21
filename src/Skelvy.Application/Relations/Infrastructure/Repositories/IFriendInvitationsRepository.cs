using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Infrastructure.Repositories
{
  public interface IFriendInvitationsRepository : IBaseRepository
  {
    Task<IList<FriendInvitation>> FindAllWithInvitingDetailsByUserId(int userId);
    Task<FriendInvitation> FindOneByRequestId(int requestId);
    Task<bool> ExistsOneByInvitingIdAndInvitedIdTwoWay(int invitingUserId, int invitedUserId);
    Task<IList<FriendInvitation>> FindAllWithRemovedByUsersId(List<int> usersId);
    Task Add(FriendInvitation invitations);
    Task Update(FriendInvitation invitation);
    Task RemoveRange(IList<FriendInvitation> invitations);
  }
}
