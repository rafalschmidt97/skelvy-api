using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Infrastructure.Facebook;
using Skelvy.Application.Core.Infrastructure.Tokens;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandHandler : IRequestHandler<SignInWithFacebookCommand, string>
  {
    private readonly SkelvyContext _context;
    private readonly IFacebookService _facebookService;
    private readonly ITokenService _tokenService;

    public SignInWithFacebookCommandHandler(
      SkelvyContext context,
      IFacebookService facebookService,
      ITokenService tokenService)
    {
      _context = context;
      _facebookService = facebookService;
      _tokenService = tokenService;
    }

    public async Task<string> Handle(SignInWithFacebookCommand request, CancellationToken cancellationToken)
    {
      var verified = await _facebookService.Verify(request.AuthToken);

      var user = _context.Users.FirstOrDefault(x => x.FacebookId == verified.UserId);

      if (user == null)
      {
        // TODO: details should also fill profile
        var details = await _facebookService.GetBody<dynamic>("me", request.AuthToken, "fields=email");

        user = new User
        {
          Email = details.email,
          FacebookId = verified.UserId
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
      }

      return _tokenService.Generate(user, verified);
    }
  }
}
