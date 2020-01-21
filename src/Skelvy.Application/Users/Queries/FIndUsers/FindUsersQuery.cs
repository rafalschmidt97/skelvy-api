using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FIndUsers
{
  public class FindUsersQuery : IQuery<IList<UserWithRelationTypeDto>>
  {
    public FindUsersQuery(int userId, string userName)
    {
      UserId = userId;
      UserName = userName;
    }

    public FindUsersQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string UserName { get; set; }
  }
}
