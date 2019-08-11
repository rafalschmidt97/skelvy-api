using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FIndUsers
{
  public class FindUsersQuery : IQuery<IList<UserWithRelationTypeDto>>
  {
    public FindUsersQuery(int userId, string userName, int page)
    {
      UserId = userId;
      UserName = userName;
      Page = page;
    }

    public FindUsersQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string UserName { get; set; }
    public int Page { get; set; }
  }
}
