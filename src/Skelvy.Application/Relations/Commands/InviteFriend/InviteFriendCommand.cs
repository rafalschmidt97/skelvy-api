using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Commands.InviteFriend
{
  public class InviteFriendCommand : ICommand
  {
    public InviteFriendCommand(int userId, int invitingUserId)
    {
      UserId = userId;
      InvitingUserId = invitingUserId;
    }

    public int UserId { get; set; }
    public int InvitingUserId { get; set; }
  }
}
