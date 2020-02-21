using Destructurama.Attributed;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommand : IQuery<AuthDto>
  {
    public SignInWithGoogleCommand(string authToken, string language)
    {
      AuthToken = authToken;
      Language = language;
    }

    [JsonConstructor]
    public SignInWithGoogleCommand()
    {
    }

    [LogMasked]
    public string AuthToken { get; set; }

    public string Language { get; set; } = LanguageType.EN;
  }
}
