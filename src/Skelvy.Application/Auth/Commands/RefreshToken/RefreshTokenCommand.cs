using Destructurama.Attributed;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommand : IQuery<Token>
  {
    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
