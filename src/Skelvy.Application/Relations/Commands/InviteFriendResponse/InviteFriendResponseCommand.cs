using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.InviteFriendResponse
{
  public class InviteFriendResponseCommand : ICommand
  {
    public InviteFriendResponseCommand(int userId, int requestId, bool isAccepted)
    {
      UserId = userId;
      RequestId = requestId;
      IsAccepted = isAccepted;
    }

    public int UserId { get; set; }
    public int RequestId { get; set; }
    public bool IsAccepted { get; set; }
  }
}
