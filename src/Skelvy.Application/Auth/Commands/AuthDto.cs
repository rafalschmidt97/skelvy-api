namespace Skelvy.Application.Auth.Commands
{
  public class AuthDto
  {
    public bool AccountCreated { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}
