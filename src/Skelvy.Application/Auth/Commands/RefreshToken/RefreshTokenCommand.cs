using Destructurama.Attributed;
using MediatR;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommand : IRequest<Token>
  {
    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
