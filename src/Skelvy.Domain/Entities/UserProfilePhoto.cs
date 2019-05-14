namespace Skelvy.Domain.Entities
{
  public class UserProfilePhoto
  {
    public UserProfilePhoto(string url, int order, int profileId)
    {
      Url = url;
      Order = order;
      ProfileId = profileId;
    }

    public UserProfilePhoto(int id, string url, int order, int profileId, UserProfile profile)
    {
      Id = id;
      Url = url;
      Order = order;
      ProfileId = profileId;
      Profile = profile;
    }

    public int Id { get; private set; }
    public string Url { get; private set; }
    public int Order { get; private set; }
    public int ProfileId { get; private set; }

    public UserProfile Profile { get; private set; }
  }
}
