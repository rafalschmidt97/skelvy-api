using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Relations.Queries.FindFriends
{
  public class FindFriendsQuery : IQuery<IList<UserDto>>
  {
    public FindFriendsQuery(int userId, int page)
    {
      UserId = userId;
      Page = page;
    }

    [JsonConstructor]
    public FindFriendsQuery()
    {
    }

    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
