using Destructurama.Attributed;
using MediatR;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommand : IRequest<string>
  {
    [LogMasked]
    public string AuthToken { get; set; }

    public string Language { get; set; }
  }
}
