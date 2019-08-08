using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.RemoveFriend
{
  public class RemoveFriendCommand : ICommand
  {
    public RemoveFriendCommand(int userId, int removedUserId)
    {
      UserId = userId;
      RemovedUserId = removedUserId;
    }

    public int UserId { get; set; }
    public int RemovedUserId { get; set; }
  }
}
