namespace Skelvy.Application.Users.Infrastructure.Notifications
{
  public class UserRemovedAction
  {
    public UserRemovedAction(int userId, string email, string language)
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
