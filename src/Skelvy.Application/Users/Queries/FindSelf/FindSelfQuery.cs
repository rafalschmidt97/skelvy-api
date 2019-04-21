using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.FindSelf
{
  public class FindSelfQuery : IQuery<SelfModel>
  {
    public FindSelfQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
