using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Events.UserDisabled
{
  public class UserDisabledEvent : IEvent
  {
    public UserDisabledEvent(int userId, string reason, string email, string language)
    {
      UserId = userId;
      Reason = reason;
      Email = email;
      Language = language;
    }

    public int UserId { get; private set; }
    public string Reason { get; private set; }
    public string Email { get; private set; }
    public string Language { get; private set; }
  }
}
