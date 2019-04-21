using Destructurama.Attributed;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommand : ICommand
  {
    public LogoutCommand(string refreshToken)
    {
      RefreshToken = refreshToken;
    }

    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
