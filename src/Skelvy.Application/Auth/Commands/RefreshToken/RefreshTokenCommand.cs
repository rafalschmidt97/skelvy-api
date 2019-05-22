using Destructurama.Attributed;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommand : IQuery<TokenDto>
  {
    public RefreshTokenCommand(string refreshToken)
    {
      RefreshToken = refreshToken;
    }

    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
