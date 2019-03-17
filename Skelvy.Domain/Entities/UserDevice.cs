namespace Skelvy.Domain.Entities
{
  public class UserDevice
  {
    public int Id { get; set; }
    public string RegistrationId { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
  }
}
