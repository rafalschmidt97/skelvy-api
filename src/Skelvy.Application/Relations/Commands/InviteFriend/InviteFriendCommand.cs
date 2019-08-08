using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.InviteFriend
{
  public class InviteFriendCommand : ICommand
  {
    public InviteFriendCommand(int userId, int invitedUserId)
    {
      UserId = userId;
      InvitedUserId = invitedUserId;
    }

    public int UserId { get; set; }
    public int InvitedUserId { get; set; }
  }
}
