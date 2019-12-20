using System;

namespace Skelvy.Domain.Entities
{
  public class RefreshToken
  {
    public RefreshToken(DateTimeOffset expiredAt, string token, int userId)
    {
      IssuedAt = DateTimeOffset.UtcNow;
      ExpiredAt = expiredAt;
      Token = token;
      UserId = userId;
    }

    public int Id { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
  }
}
