using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.RemoveFriend
{
  public class RemoveFriendCommand : ICommand
  {
    public RemoveFriendCommand(int userId, int friendUserId)
    {
      UserId = userId;
      FriendUserId = friendUserId;
    }

    [JsonConstructor]
    public RemoveFriendCommand()
    {
    }

    public int UserId { get; set; }
    public int FriendUserId { get; set; }
  }
}
