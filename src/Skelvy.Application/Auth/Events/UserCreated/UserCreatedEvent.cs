using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Auth.Events.UserCreated
{
  public class UserCreatedEvent : IEvent
  {
    public UserCreatedEvent(int userId, string email, string language)
    {
      UserId = userId;
      Email = email;
      Language = language;
    }

    public int UserId { get; private set; }
    public string Email { get; private set; }
    public string Language { get; private set; }
  }
}
