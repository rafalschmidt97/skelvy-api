namespace Skelvy.Domain.Entities
{
  public class UserProfilePhoto
  {
    public UserProfilePhoto(string url, int profileId)
    {
      Url = url;
      ProfileId = profileId;
    }

    public UserProfilePhoto(int id, string url, int profileId)
      : this(url, profileId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public string Url { get; private set; }
    public int ProfileId { get; private set; }

    public UserProfile Profile { get; private set; }
  }
}
