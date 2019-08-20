using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.AddBlocked
{
  public class AddBlockedCommand : ICommand
  {
    public AddBlockedCommand(int userId, int blockingUserId)
    {
      UserId = userId;
      BlockingUserId = blockingUserId;
    }

    public int UserId { get; set; }
    public int BlockingUserId { get; set; }
  }
}
