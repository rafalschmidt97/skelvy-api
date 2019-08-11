using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.CheckUserName
{
  public class CheckUserNameQuery : IQuery<bool>
  {
    public CheckUserNameQuery(string name)
    {
      Name = name;
    }

    public CheckUserNameQuery() // required for FromQuery attribute
    {
    }

    public string Name { get; set; }
  }
}
