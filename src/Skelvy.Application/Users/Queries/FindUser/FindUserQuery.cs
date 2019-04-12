using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQuery : IQuery<UserDto>
  {
    public int Id { get; set; }
  }
}
