using Destructurama.Attributed;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommand : IQuery<AuthDto>
  {
    public SignInWithGoogleCommand(string authToken, string language)
    {
      AuthToken = authToken;
      Language = language;
    }

    public SignInWithGoogleCommand() // required for FromBody attribute to allow default values
    {
    }

    [LogMasked]
    public string AuthToken { get; set; }

    public string Language { get; set; } = LanguageType.EN;
  }
}
