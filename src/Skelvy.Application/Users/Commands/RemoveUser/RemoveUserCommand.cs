using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommand : ICommand
  {
    public RemoveUserCommand(int id)
    {
      Id = id;
    }

    public int Id { get; set; }
  }
}
