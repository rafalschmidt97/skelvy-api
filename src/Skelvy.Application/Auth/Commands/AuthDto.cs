namespace Skelvy.Application.Auth.Commands
{
  public class AuthDto
  {
    public AuthDto(string accessToken, string refreshToken)
    {
      AccessToken = accessToken;
      RefreshToken = refreshToken;
    }

    public string AccessToken { get; }
    public string RefreshToken { get; }
  }
}
