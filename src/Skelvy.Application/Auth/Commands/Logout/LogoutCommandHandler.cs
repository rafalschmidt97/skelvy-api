using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
  {
    private readonly ITokenService _tokenService;

    public LogoutCommandHandler(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
      await _tokenService.Invalidate(request.RefreshToken, cancellationToken);
      return Unit.Value;
    }
  }
}
