using Destructurama.Attributed;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommand : ICommand
  {
    public LogoutCommand(string refreshToken)
    {
      RefreshToken = refreshToken;
    }

    [JsonConstructor]
    public LogoutCommand()
    {
    }

    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
