using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQuery : IQuery<UserDto>
  {
    public FindUserQuery(int id)
    {
      Id = id;
    }

    public int Id { get; set; }
  }
}
