using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQuery : IQuery<UserDto>
  {
    public FindUserQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
