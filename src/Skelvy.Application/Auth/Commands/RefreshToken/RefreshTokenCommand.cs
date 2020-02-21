using Destructurama.Attributed;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommand : IQuery<TokenDto>
  {
    public RefreshTokenCommand(string refreshToken)
    {
      RefreshToken = refreshToken;
    }

    [JsonConstructor]
    public RefreshTokenCommand()
    {
    }

    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
