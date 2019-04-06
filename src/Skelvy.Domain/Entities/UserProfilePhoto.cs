namespace Skelvy.Domain.Entities
{
  public class UserProfilePhoto
  {
    public int Id { get; set; }
    public string Url { get; set; }
    public string Status { get; set; }
    public int ProfileId { get; set; }

    public UserProfile Profile { get; set; }
  }
}
