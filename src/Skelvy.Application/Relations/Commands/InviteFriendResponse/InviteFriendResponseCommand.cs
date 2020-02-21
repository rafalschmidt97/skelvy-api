using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.InviteFriendResponse
{
  public class InviteFriendResponseCommand : ICommand
  {
    public InviteFriendResponseCommand(int userId, int invitationId, bool isAccepted)
    {
      UserId = userId;
      InvitationId = invitationId;
      IsAccepted = isAccepted;
    }

    [JsonConstructor]
    public InviteFriendResponseCommand()
    {
    }

    public int UserId { get; set; }
    public int InvitationId { get; set; }
    public bool IsAccepted { get; set; }
  }
}
