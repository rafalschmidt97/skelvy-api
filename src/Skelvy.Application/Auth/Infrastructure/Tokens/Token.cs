using System;

namespace Skelvy.Application.Auth.Infrastructure.Tokens
{
  public class Token
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }

  [Serializable]
  public class TokenUser
  {
    public int Id { get; set; }
  }
}
