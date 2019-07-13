using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindSelfUser
{
  public class FindSelfUserQuery : IQuery<SelfUserDto>
  {
    public FindSelfUserQuery(int id)
    {
      Id = id;
    }

    public int Id { get; set; }
  }
}
