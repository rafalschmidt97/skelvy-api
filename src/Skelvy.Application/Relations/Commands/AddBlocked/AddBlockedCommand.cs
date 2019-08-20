using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.AddBlocked
{
  public class AddBlockedCommand : ICommand
  {
    public AddBlockedCommand(int userId, int relatedUserId)
    {
      UserId = userId;
      RelatedUserId = relatedUserId;
    }

    public int UserId { get; set; }
    public int RelatedUserId { get; set; }
  }
}
