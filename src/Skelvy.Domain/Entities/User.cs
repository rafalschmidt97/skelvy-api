using System.Collections.Generic;

namespace Skelvy.Domain.Entities
{
  public class User
  {
    public User()
    {
      Roles = new List<UserRole>();
      MeetingChatMessages = new List<MeetingChatMessage>();
      MeetingRequests = new List<MeetingRequest>();
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }

    public UserProfile Profile { get; set; }
    public IList<UserRole> Roles { get; private set; }
    public IList<MeetingChatMessage> MeetingChatMessages { get; private set; }
  }
}
