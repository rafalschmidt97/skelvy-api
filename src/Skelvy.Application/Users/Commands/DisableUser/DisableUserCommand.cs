using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommand : ICommand
  {
    public DisableUserCommand(int id, string reason)
    {
      Id = id;
      Reason = reason;
    }

    public int Id { get; set; }
    public string Reason { get; set; }
  }
}
