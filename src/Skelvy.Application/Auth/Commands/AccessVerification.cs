using System;

namespace Skelvy.Application.Auth.Commands
{
  public class AccessVerification
  {
    public AccessVerification(string userId, string accessToken, DateTimeOffset expiresAt, string accessType)
    {
      UserId = userId;
      AccessToken = accessToken;
      ExpiresAt = expiresAt;
      AccessType = accessType;
    }

    public string UserId { get; }
    public string AccessToken { get; }
    public DateTimeOffset ExpiresAt { get; }
    public string AccessType { get; }
  }
}
