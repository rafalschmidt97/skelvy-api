using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommand : ICommand
  {
    public DisableUserCommand(int userId, string reason)
    {
      UserId = userId;
      Reason = reason;
    }

    [JsonConstructor]
    public DisableUserCommand()
    {
    }

    public int UserId { get; set; }
    public string Reason { get; set; }
  }
}
