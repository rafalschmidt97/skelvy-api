using System.Collections.Generic;

namespace Skelvy.Domain.Entities
{
  public class User
  {
    public User()
    {
      MeetingChatMessages = new List<MeetingChatMessage>();
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }

    public UserProfile Profile { get; set; }
    public MeetingRequest MeetingRequest { get; set; }
    public IList<MeetingChatMessage> MeetingChatMessages { get; private set; }
  }
}
