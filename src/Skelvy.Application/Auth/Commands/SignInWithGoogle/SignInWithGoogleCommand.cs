using Destructurama.Attributed;
using MediatR;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommand : IRequest<Token>
  {
    [LogMasked]
    public string AuthToken { get; set; }

    public string Language { get; set; }
  }
}
