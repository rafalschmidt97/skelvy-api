using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.AddBlockedUser
{
  public class AddBlockedUserCommand : ICommand
  {
    public AddBlockedUserCommand(int userId, int blockUserId)
    {
      UserId = userId;
      BlockUserId = blockUserId;
    }

    public int UserId { get; set; }
    public int BlockUserId { get; set; }
  }
}
