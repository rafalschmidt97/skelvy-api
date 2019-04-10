using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Token>
  {
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    public async Task<Token> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
      return await _tokenService.Generate(request.RefreshToken, cancellationToken);
    }
  }
}
