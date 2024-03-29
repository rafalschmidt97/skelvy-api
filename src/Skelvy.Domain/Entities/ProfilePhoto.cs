namespace Skelvy.Domain.Entities
{
  public class ProfilePhoto
  {
    public ProfilePhoto(int attachmentId, int order, int profileId)
    {
      AttachmentId = attachmentId;
      Order = order;
      ProfileId = profileId;
    }

    public int Id { get; set; }
    public int AttachmentId { get; set; }
    public int Order { get; set; }
    public int ProfileId { get; set; }

    public Profile Profile { get; set; }
    public Attachment Attachment { get; set; }
  }
}
