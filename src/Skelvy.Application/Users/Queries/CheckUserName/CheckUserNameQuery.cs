using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Queries.CheckUserName
{
  public class CheckUserNameQuery : IQuery<bool>
  {
    public CheckUserNameQuery(string name)
    {
      Name = name;
    }

    [JsonConstructor]
    public CheckUserNameQuery()
    {
    }

    public string Name { get; set; }
  }
}
