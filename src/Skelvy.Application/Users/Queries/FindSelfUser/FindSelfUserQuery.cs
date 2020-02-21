using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindSelfUser
{
  public class FindSelfUserQuery : IQuery<SelfUserDto>
  {
    public FindSelfUserQuery(int userId)
    {
      UserId = userId;
    }

    [JsonConstructor]
    public FindSelfUserQuery()
    {
    }

    public int UserId { get; set; }
  }
}
