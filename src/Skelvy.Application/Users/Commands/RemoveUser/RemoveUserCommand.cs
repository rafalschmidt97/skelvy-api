using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommand : ICommand
  {
    public RemoveUserCommand(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
