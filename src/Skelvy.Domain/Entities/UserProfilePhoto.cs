namespace Skelvy.Domain.Entities
{
  public class UserProfilePhoto
  {
    public UserProfilePhoto(int attachmentId, int order, int profileId)
    {
      AttachmentId = attachmentId;
      Order = order;
      ProfileId = profileId;
    }

    public UserProfilePhoto(int id, int attachmentId, int order, int profileId, UserProfile profile, Attachment attachment)
    {
      Id = id;
      AttachmentId = attachmentId;
      Order = order;
      ProfileId = profileId;
      Profile = profile;
      Attachment = attachment;
    }

    public int Id { get; private set; }
    public int AttachmentId { get; private set; }
    public int Order { get; private set; }
    public int ProfileId { get; private set; }

    public UserProfile Profile { get; private set; }
    public Attachment Attachment { get; private set; }
  }
}
