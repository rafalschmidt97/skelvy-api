using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class SocketMessage
  {
    [JsonConstructor]
    public SocketMessage(IEnumerable<int> usersId, string action, object data = null)
    {
      UsersId = usersId;
      Action = action;
      Data = data;
    }

    public SocketMessage(int userId, string action, object data = null)
      : this(new[] { userId }, action, data)
    {
    }

    public IEnumerable<int> UsersId { get; private set; }
    public string Action { get; private set; }
    public object Data { get; private set; }
  }
}
