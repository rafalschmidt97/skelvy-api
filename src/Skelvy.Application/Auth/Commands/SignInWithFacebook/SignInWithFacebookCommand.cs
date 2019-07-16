using Destructurama.Attributed;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommand : IQuery<AuthDto>
  {
    public SignInWithFacebookCommand(string authToken, string language)
    {
      AuthToken = authToken;
      Language = language;
    }

    public SignInWithFacebookCommand() // required for FromBody attribute to allow default values
    {
    }

    [LogMasked]
    public string AuthToken { get; set; }

    public string Language { get; set; } = LanguageTypes.EN;
  }
}
