using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.RemoveBlocked
{
  public class RemoveBlockedCommand : ICommand
  {
    public RemoveBlockedCommand(int userId, int blockedUserId)
    {
      UserId = userId;
      BlockedUserId = blockedUserId;
    }

    [JsonConstructor]
    public RemoveBlockedCommand()
    {
    }

    public int UserId { get; set; }
    public int BlockedUserId { get; set; }
  }
}
