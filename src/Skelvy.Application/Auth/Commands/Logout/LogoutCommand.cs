using Destructurama.Attributed;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommand : ICommand
  {
    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
