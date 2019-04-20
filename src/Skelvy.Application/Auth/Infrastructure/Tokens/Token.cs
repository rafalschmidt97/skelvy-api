using System;

namespace Skelvy.Application.Auth.Infrastructure.Tokens
{
  public class Token
  {
    public Token(string accessToken, string refreshToken)
    {
      AccessToken = accessToken;
      RefreshToken = refreshToken;
    }

    public string AccessToken { get; }
    public string RefreshToken { get; }
  }

  [Serializable]
  public class TokenUser
  {
    public TokenUser(int id)
    {
      Id = id;
    }

    public int Id { get; }
  }
}
