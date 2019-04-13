using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommandHandler : CommandHandler<LogoutCommand>
  {
    private readonly ITokenService _tokenService;

    public LogoutCommandHandler(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    public override async Task<Unit> Handle(LogoutCommand request)
    {
      await _tokenService.Invalidate(request.RefreshToken);
      return Unit.Value;
    }
  }
}
