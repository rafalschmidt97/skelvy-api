using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.RemoveBlocked
{
  public class RemoveBlockedCommand : ICommand
  {
    public RemoveBlockedCommand(int userId, int relatedUserId)
    {
      UserId = userId;
      RelatedUserId = relatedUserId;
    }

    public int UserId { get; set; }
    public int RelatedUserId { get; set; }
  }
}
