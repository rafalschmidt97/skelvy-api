using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.RemoveBlockedUser
{
  public class RemoveBlockedUserCommand : ICommand
  {
    public RemoveBlockedUserCommand(int userId, int blockUserId)
    {
      UserId = userId;
      BlockUserId = blockUserId;
    }

    public int UserId { get; set; }
    public int BlockUserId { get; set; }
  }
}
