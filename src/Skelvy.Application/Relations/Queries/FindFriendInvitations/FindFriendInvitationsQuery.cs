using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Queries.FindFriendInvitations
{
  public class FindFriendInvitationsQuery : IQuery<IList<FriendInvitationsDto>>
  {
    public FindFriendInvitationsQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
