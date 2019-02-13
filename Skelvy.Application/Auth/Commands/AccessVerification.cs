using System;

namespace Skelvy.Application.Auth.Commands
{
  public class AccessVerification
  {
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string AccessType { get; set; }
  }
}
