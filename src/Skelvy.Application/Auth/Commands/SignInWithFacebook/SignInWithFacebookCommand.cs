using Destructurama.Attributed;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommand : IQuery<Token>
  {
    [LogMasked]
    public string AuthToken { get; set; }

    public string Language { get; set; }
  }
}
