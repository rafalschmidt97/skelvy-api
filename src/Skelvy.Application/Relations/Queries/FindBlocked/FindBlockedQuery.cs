using System.Collections.Generic;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Relations.Queries.FindBlocked
{
  public class FindBlockedQuery : IQuery<IList<UserDto>>
  {
    public FindBlockedQuery(int userId, int page)
    {
      UserId = userId;
      Page = page;
    }

    public FindBlockedQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
