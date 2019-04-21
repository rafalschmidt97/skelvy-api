using System;

namespace Skelvy.Application.Auth.Infrastructure.Tokens
{
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
