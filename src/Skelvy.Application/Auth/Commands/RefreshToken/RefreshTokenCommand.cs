using Destructurama.Attributed;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommand : IQuery<Token>
  {
    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
