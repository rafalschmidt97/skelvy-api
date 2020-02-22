using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FIndUsers
{
  public class FindUsersQuery : IQuery<IList<UserDto>>
  {
    public FindUsersQuery(int userId, string userName)
    {
      UserId = userId;
      UserName = userName;
    }

    [JsonConstructor]
    public FindUsersQuery()
    {
    }

    public int UserId { get; set; }
    public string UserName { get; set; }
  }
}
