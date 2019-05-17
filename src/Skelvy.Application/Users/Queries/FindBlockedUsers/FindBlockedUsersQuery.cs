using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindBlockedUsers
{
  public class FindBlockedUsersQuery : IQuery<IList<UserDto>>
  {
    public FindBlockedUsersQuery(int userId, int page)
    {
      UserId = userId;
      Page = page;
    }

    public FindBlockedUsersQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
