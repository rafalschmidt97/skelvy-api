using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Queries.FindFriendRequests
{
  public class FindFriendRequestsQuery : IQuery<IList<FriendRequestDto>>
  {
    public FindFriendRequestsQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
