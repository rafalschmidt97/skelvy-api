using System.Threading.Tasks;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommandHandler : QueryHandler<RefreshTokenCommand, Token>
  {
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    public override async Task<Token> Handle(RefreshTokenCommand request)
    {
      return await _tokenService.Generate(request.RefreshToken);
    }
  }
}
